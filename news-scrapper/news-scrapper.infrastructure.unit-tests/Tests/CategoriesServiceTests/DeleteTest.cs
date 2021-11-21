using FluentAssertions;
using Moq;
using news_scrapper.domain.DBModels;
using news_scrapper.resources;
using System;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.CategoriesServiceTests
{
    public class DeleteTest : BaseTest
    {
        [Fact]
        public void should_delete_category()
        {
            int categoryId = 1;
            CategoryDb db = new();

            _categories.Setup(n => n.GetById(categoryId)).Returns(db);

            _sut.Delete(categoryId);

            _categories.Verify(n => n.Delete(db), Times.Once);
        }

        [Fact]
        public void should_commit_changes()
        {
            _categories.Setup(n => n.GetById(1)).Returns(new CategoryDb());

            _sut.Delete(1);

            _categoriesUnitOfWork.Verify(n => n.Commit(), Times.Once);
        }

        [Fact]
        public void should_return_true()
        {
            _categories.Setup(n => n.GetById(1)).Returns(new CategoryDb());

            var result = _sut.Delete(1);

            result.Should().BeTrue();
        }
                
        [Fact]
        public void should_throw_exception_when_entity_not_found()
        {
            CategoryDb categoryDb = null;

            _categories.Setup(n=> n.GetById(1)).Returns(categoryDb);

            _sut.Invoking(n => n.Delete(1))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(string.Format(ApiResponses.CategoryWithGivenIdNotFound, 1));
        }
    }
}
