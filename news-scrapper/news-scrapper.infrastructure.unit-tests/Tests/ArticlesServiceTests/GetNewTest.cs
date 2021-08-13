using FluentAssertions;
using Moq;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models;
using news_scrapper.infrastructure.unit_tests.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.ArticlesServiceTests
{
    public class GetNewTest : BaseTest
    {
        [Fact]
        public void should_get_articles_from_the_latest_scrap()
        {
            List<ArticleDb> articlesFromDb = new ArticleBuilder().Build(5).Map().OrderByDescending(n => n.DateScrapped).ToList();

            var newestScrappingDate = articlesFromDb.First().DateScrapped;
            var expectedResult = articlesFromDb.Where(n => n.DateScrapped == newestScrappingDate).ToList();

            List<ArticleDb> mappedArticles = null;
            Action<object> mapArticles = (list) => { mappedArticles = list as List<ArticleDb>; };

            _articlesRepository.Setup(n => n.Get(null, It.IsAny<Func<IQueryable<ArticleDb>, IOrderedQueryable<ArticleDb>>>(), ""))
                .Returns(articlesFromDb);
            _mapper.Setup(n => n.Map<List<Article>>(It.IsAny<List<ArticleDb>>())).Callback(mapArticles);

            _sut.GetNew();

            mappedArticles.Should().BeEquivalentTo(expectedResult);
        }

        public static IEnumerable<object[]> EmptyArticlesFromDb =>
            new List<object[]> { 
                new object[] { null },
                new object[] { new List<ArticleDb>() }
            };

        [Theory]
        [MemberData(nameof(EmptyArticlesFromDb))]
        public void should_return_null_when_no_articles_in_db(List<ArticleDb> articlesFromDb)
        {
            _articlesRepository.Setup(n => n.Get(null, It.IsAny<Func<IQueryable<ArticleDb>, IOrderedQueryable<ArticleDb>>>(), ""))
                .Returns(articlesFromDb);

            var result = _sut.GetNew();

            result.Should().BeEquivalentTo(null);
        }
    }
}
