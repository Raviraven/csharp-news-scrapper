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
    public class GetTest : BaseTest
    {
        [Fact]
        public void should_return_articles_from_db()
        {
            List<ArticleDb> articlesFromDb = new ArticleBuilder().Build(10).Map();

            List<ArticleDb> mappedArticles = null;
            Action<object> mapResult = (list) => { mappedArticles = list as List<ArticleDb>; };

            _articlesRepository.Setup(n => n.Get(null, It.IsAny<Func<IQueryable<ArticleDb>, IOrderedQueryable<ArticleDb>>>(), ""))
                .Returns(articlesFromDb);
            _mapper.Setup(n => n.Map<List<Article>>(It.IsAny<List<ArticleDb>>())).Callback(mapResult);

            _sut.Get();

            mappedArticles.Should().BeEquivalentTo(articlesFromDb);
        }

        public static IEnumerable<object[]> EmptyArticlesFromDb =>
            new List<object[]>
            {
                new object[] { null },
                new object[] { new List<ArticleDb>() }
            };

        [Theory]
        [MemberData(nameof(EmptyArticlesFromDb))]
        public void should_return_null_when_no_articles_in_db(List<ArticleDb> articlesFromDb)
        {
            List<ArticleDb> expectedResult = new();

            List<ArticleDb> mappedArticles = null;
            Action<object> mapResult = (list) => { mappedArticles = list as List<ArticleDb>; };

            _articlesRepository.Setup(n => n.Get(null, It.IsAny<Func<IQueryable<ArticleDb>, IOrderedQueryable<ArticleDb>>>(), ""))
                .Returns(articlesFromDb);
            _mapper.Setup(n => n.Map<List<Article>>(It.IsAny<List<ArticleDb>>())).Callback(mapResult);

            var result = _sut.Get();

            result.Should().BeNull();
        }


        //with parameters for paging

        [Fact]
        public void should_return_articles()
        {
            var articlesFromDb = new ArticleBuilder().Build(5).Map();

            List<ArticleDb> mappedArticles = null;
            Action<object> mapArticles = (list) => { mappedArticles = list as List<ArticleDb>; };

            _articlesRepository.Setup(n => n.Get(null, It.IsAny<Func<IQueryable<ArticleDb>, IOrderedQueryable<ArticleDb>>>(), ""))
                .Returns(articlesFromDb);
            _mapper.Setup(n => n.Map<List<Article>>(It.IsAny<List<ArticleDb>>())).Callback(mapArticles);

            _sut.Get(5, 1, -1);

            mappedArticles.Should().BeEquivalentTo(articlesFromDb);
        }

        [Fact]
        public void should_return_articles_from_middle_page()
        {
            var articlesFromDb = new ArticleBuilder().Build(15).Map();
            List<ArticleDb> filteredArticles = articlesFromDb.Skip(5).Take(5).ToList();

            List<ArticleDb> mappedArticles = null;
            Action<object> mapArticles = (list) => { mappedArticles = list as List<ArticleDb>; };

            _articlesRepository.Setup(n => n.Get(null, It.IsAny<Func<IQueryable<ArticleDb>, IOrderedQueryable<ArticleDb>>>(), ""))
                .Returns(articlesFromDb);
            _mapper.Setup(n => n.Map<List<Article>>(It.IsAny<List<ArticleDb>>())).Callback(mapArticles);

            _sut.Get(5, 2, -1);

            mappedArticles.Should().BeEquivalentTo(filteredArticles);
        }

        [Fact]
        public void should_return_articles_from_last_page()
        {
            var articlesFromDb = new ArticleBuilder().Build(15).Map();
            List<ArticleDb> filteredArticles = articlesFromDb.Skip(10).Take(5).ToList();

            List<ArticleDb> mappedArticles = null;
            Action<object> mapArticles = (list) => { mappedArticles = list as List<ArticleDb>; };

            _articlesRepository.Setup(n => n.Get(null, It.IsAny<Func<IQueryable<ArticleDb>, IOrderedQueryable<ArticleDb>>>(), ""))
                .Returns(articlesFromDb);
            _mapper.Setup(n => n.Map<List<Article>>(It.IsAny<List<ArticleDb>>())).Callback(mapArticles);

            _sut.Get(5, 3, -1);

            mappedArticles.Should().BeEquivalentTo(filteredArticles);
        }

        public static IEnumerable<object[]> EmptyArticlesDB =>
            new List<object[]>
            {
                new object[] { null },
                new object[] { new List<ArticleDb>() }
            };

        [Theory]
        [MemberData(nameof(EmptyArticlesDB))]
        public void should_return_null_if_no_articles_in_db(List<ArticleDb> articlesFromDb)
        {
            _articlesRepository.Setup(n => n.Get(null, It.IsAny<Func<IQueryable<ArticleDb>, IOrderedQueryable<ArticleDb>>>(), ""))
                .Returns(articlesFromDb);

            var result = _sut.Get(0, 0, -1);

            result.Should().BeNull();
        }

        [Fact]
        public void should_return_null_if_wrong_page_passed()
        {
            List<ArticleDb> articlesFromDb = new ArticleBuilder().Build(9).Map();
            
            _articlesRepository.Setup(n => n.Get(null, It.IsAny<Func<IQueryable<ArticleDb>, IOrderedQueryable<ArticleDb>>>(), ""))
               .Returns(articlesFromDb);

            var result = _sut.Get(5, 3, -1);

            result.Should().BeNull();
        }

        [Fact]
        public void should_return_all_articles_when_articles_per_page_is_higher_than_articles_in_db()
        {
            List<ArticleDb> articlesFromDb = new ArticleBuilder().Build(4).Map();

            List<ArticleDb> mappedArticles = null;
            Action<object> mapArticles = (list) => { mappedArticles = list as List<ArticleDb>; };

            _articlesRepository.Setup(n => n.Get(null, It.IsAny<Func<IQueryable<ArticleDb>, IOrderedQueryable<ArticleDb>>>(), ""))
               .Returns(articlesFromDb);
            _mapper.Setup(n => n.Map<List<Article>>(It.IsAny<List<ArticleDb>>())).Callback(mapArticles);

            _sut.Get(10, 1, -1);

            mappedArticles.Should().BeEquivalentTo(articlesFromDb);
        }
    }
}
