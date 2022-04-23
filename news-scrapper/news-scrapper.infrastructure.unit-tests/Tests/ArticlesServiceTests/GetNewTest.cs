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
        public void should_get_the_newest_articles_from_entire_day()
        {
            var articles = new List<Article>
            {
                new ArticleBuilder().WithDate(new DateTime(2021, 03, 12)).Build(),
                new ArticleBuilder().WithDate(new DateTime(2022,04,22)).Build(),
                new ArticleBuilder().WithDate(new DateTime(2022, 04, 23,10, 20, 02)).Build(),
                new ArticleBuilder().WithDate(new DateTime(2022, 04, 23, 10, 20, 21)).Build(),
                new ArticleBuilder().WithDate(new DateTime(2022, 04, 23,12, 00 , 00)).Build(),
                new ArticleBuilder().WithDate(new DateTime(2022, 04, 23, 15, 40, 00)).Build(),
                new ArticleBuilder().WithDate(new DateTime(2022, 04, 23, 23, 00, 41)).Build(),
            };
            var articlesFromDb = articles.Map().OrderByDescending(n => n.DateScrapped).ToList();

            var expectedResult = articles
                .Where(a => a.DateScrapped.ToShortDateString() == new DateTime(2022, 04, 23).ToShortDateString())
                .OrderByDescending(a => a.DateScrapped)
                .ToList();

            List<ArticleDb> mappedArticles = null;

            void MapArticles(object list)
            {
                mappedArticles = list as List<ArticleDb>;
            }

            _articlesRepository.Setup(n => n.Get(null, It.IsAny<Func<IQueryable<ArticleDb>, IOrderedQueryable<ArticleDb>>>(), ""))
                .Returns(articlesFromDb);
            _mapper.Setup(n => n.Map<List<Article>>(It.IsAny<List<ArticleDb>>())).Callback((Action<object>) MapArticles);

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
