using FluentAssertions;
using Moq;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models;
using news_scrapper.domain.ResponseViewModels;
using news_scrapper.infrastructure.unit_tests.Builders;
using news_scrapper.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.ArticlesServiceTests
{
    public class ScrapTest : BaseTest
    {
        [Fact]
        public async void should_add_scrapped_articles_into_db()
        {
            var scrappedArticles = new ArticleBuilder().Build(10);
            ArticlesResponseViewModel scrappedArticlesResponse = new() { 
                Articles = scrappedArticles
            };
            List<ArticleDb> articleInDb = new();

            List<ArticleDb> insertedArticles = null;
            Action<List<ArticleDb>> insertRange = (list) => { insertedArticles = list; };

            List<ArticleDb> expectedResult = scrappedArticles.Map();

            _pagesScrapperService.Setup(n => n.ScrapAll()).ReturnsAsync(scrappedArticlesResponse);
            _articlesRepository.Setup(n => n.Get(null, null, "")).Returns(articleInDb);
            _articlesRepository.Setup(n => n.InsertRange(It.IsAny<List<ArticleDb>>())).Callback(insertRange);
            _mapper.Setup(n => n.Map<List<ArticleDb>>(It.IsAny<List<Article>>())).Returns(scrappedArticles.Map());

            await _sut.Scrap();

            insertedArticles.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void should_return_summary_message()
        {
            int articlesCount = 5;
            string summaryMessage = string.Format(ApiResponses.ArticlesAddedAfterScrapping, articlesCount);
            List<string> expectedResult = new() { summaryMessage };

            var scrappedArticles = new ArticleBuilder().Build(articlesCount);
            ArticlesResponseViewModel scrappedArticlesResponse = new()
            {
                Articles = scrappedArticles
            };
            List<ArticleDb> articleInDb = new();

            _pagesScrapperService.Setup(n => n.ScrapAll()).ReturnsAsync(scrappedArticlesResponse);
            _articlesRepository.Setup(n => n.Get(null, null, "")).Returns(articleInDb);
            _mapper.Setup(n => n.Map<List<ArticleDb>>(It.IsAny<List<Article>>())).Returns(scrappedArticles.Map());

            var result = await _sut.Scrap();

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void should_add_only_new_articles()
        {
            int scrappedArticlesCount = 5;
            int articlesInDbCount = 2;

            var scrappedArticles = new ArticleBuilder().Build(scrappedArticlesCount);
            ArticlesResponseViewModel scrappedArticlesResponse = new()
            {
                Articles = scrappedArticles
            };

            List<ArticleDb> articlesInDb = scrappedArticles.Take(articlesInDbCount).ToList().Map();

            List<Article> articlesToAdd = null;
            Action<object> mapArticlesToAdd = (list) => { articlesToAdd = list as List<Article>; };

            List<Article> expectedResult = scrappedArticles.TakeLast(3).ToList();

            _pagesScrapperService.Setup(n => n.ScrapAll()).ReturnsAsync(scrappedArticlesResponse);
            _articlesRepository.Setup(n => n.Get(null, null, "")).Returns(articlesInDb);
            _mapper.Setup(n => n.Map<List<ArticleDb>>(It.IsAny<List<Article>>())).Callback(mapArticlesToAdd);

            await _sut.Scrap();

            articlesToAdd.Should().BeEquivalentTo(expectedResult);
        }

        public static IEnumerable<object[]> emptyScrappedArticles =>
            new List<object[]>
            {
                new object[] { null },
                new object[] { new List<Article>() }
            };

        [Theory]
        [MemberData(nameof(emptyScrappedArticles))]
        public async void should_not_add_anything_to_db_when_scrapping_doesnt_return_any_articles(List<Article> scrappedArticles)
        {
            ArticlesResponseViewModel scrappedArticlesResponse = new()
            {
                Articles = scrappedArticles
            };

            List<Article> articlesToAdd = null;
            Action<object> mapArticlesToAdd = (list) => { articlesToAdd = list as List<Article>; };
            List<Article> expectedResult = new();

            _pagesScrapperService.Setup(n => n.ScrapAll()).ReturnsAsync(scrappedArticlesResponse);
            _mapper.Setup(n => n.Map<List<ArticleDb>>(It.IsAny<List<Article>>())).Callback(mapArticlesToAdd);

            await _sut.Scrap();

            articlesToAdd.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void should_not_add_articles_to_db_when_articles_are_already_in_db()
        {
            var scrappedArticles = new ArticleBuilder().Build(10);
            ArticlesResponseViewModel scrappedArticlesResponse = new()
            {
                Articles = scrappedArticles
            };
            List<ArticleDb> articlesInDb = scrappedArticles.Map();

            List<Article> articlesToAdd = null;
            Action<object> mapArticlesToAdd = (list) => { articlesToAdd = list as List<Article>; };

            List<ArticleDb> expectedResult = new();

            _pagesScrapperService.Setup(n => n.ScrapAll()).ReturnsAsync(scrappedArticlesResponse);
            _articlesRepository.Setup(n => n.Get(null, null, ""))
                .Returns(articlesInDb);
            _mapper.Setup(n => n.Map<List<ArticleDb>>(It.IsAny<List<Article>>())).Callback(mapArticlesToAdd);

            await _sut.Scrap();

            articlesToAdd.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void should_return_error_messages_from_scrapping()
        {
            ArticlesResponseViewModel scrappedArticlesResult = new()
            {
                Articles = null,
                ErrorMessages = new() { "error 1", "error 2" }
            };

            string summaryMessage = string.Format(ApiResponses.ArticlesAddedAfterScrapping, 0);

            List<string> expectedResult = new() { "error 1", "error 2", summaryMessage };

            _pagesScrapperService.Setup(n => n.ScrapAll()).ReturnsAsync(scrappedArticlesResult);

            var result = await _sut.Scrap();

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void should_return_error_messages_when_error_occured()
        {
            ArticlesResponseViewModel scrappedArticlesResult = new();
            string exceptionMessage = "Error occured while trying to insert articles into db";
            string summaryMessage = string.Format(ApiResponses.ArticlesAddedAfterScrapping, 0);
            List<string> expectedResult = new() { exceptionMessage, summaryMessage };

            _pagesScrapperService.Setup(n => n.ScrapAll()).ReturnsAsync(scrappedArticlesResult);
            _articlesRepository.Setup(n => n.InsertRange(It.IsAny<List<ArticleDb>>())).Throws(new Exception(exceptionMessage));

            var result = await _sut.Scrap();

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
