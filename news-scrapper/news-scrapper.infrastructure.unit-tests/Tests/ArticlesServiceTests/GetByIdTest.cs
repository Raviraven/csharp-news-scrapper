using FluentAssertions;
using Moq;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models;
using news_scrapper.infrastructure.unit_tests.Builders;
using System;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.ArticlesServiceTests
{
    public class GetByIdTest : BaseTest
    {
        [Fact]
        public void should_return_article()
        {
            ArticleDb articleFromDb = new ArticleBuilder().Build().Map();
            ArticleDb mappedArticle = null;

            Action<object> mapArticle = (item) => { mappedArticle = item as ArticleDb; };

            _articlesRepository.Setup(n => n.GetById(articleFromDb.Id)).Returns(articleFromDb);
            _mapper.Setup(n => n.Map<Article>(It.IsAny<ArticleDb>())).Callback(mapArticle);

            _sut.GetById(articleFromDb.Id);

            mappedArticle.Should().BeEquivalentTo(articleFromDb);
        }

        [Fact]
        public void should_return_null_when_article_not_found()
        {
            ArticleDb articleFromDb = new ArticleBuilder().Build().Map();
            ArticleDb mappedArticle = null;

            Action<object> mapArticle = (item) => { mappedArticle = item as ArticleDb; };

            _articlesRepository.Setup(n => n.GetById(articleFromDb.Id)).Returns(articleFromDb);
            _mapper.Setup(n => n.Map<Article>(It.IsAny<ArticleDb>())).Callback(mapArticle);

            var result = _sut.GetById(articleFromDb.Id);

            result.Should().BeNull();
        }
    }
}
