using FluentAssertions;
using Moq;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models.Categories;
using news_scrapper.infrastructure.unit_tests.Extensions;
using news_scrapper.resources;
using System;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.CategoriesServiceTests
{
    public class SaveTest :  BaseTest
    {
        [Fact]
        public void should_save_category_in_db()
        {
            CategoryEdit categoryEdit = new()
            {
                Id = -123,
                Name = "test name",
            };
            CategoryDb categoryDb = categoryEdit.MapToCategoryDb();

            _categories.Setup(n => n.Update(categoryDb)).Returns(categoryDb);
            _mapper.Setup(n => n.Map<CategoryDb>(categoryEdit)).Returns(categoryDb);

            _sut.Save(categoryEdit);

            _categories.Verify(n=>n.Update(categoryDb));
        }

        [Fact]
        public void should_return_saved_category()
        {
            CategoryEdit categoryEdit = new()
            {
                Id = -123,
                Name = "test name",
            };
            CategoryDb categoryDb = categoryEdit.MapToCategoryDb();
            Category mappedCategory = categoryDb.MapToCategory();

            _categories.Setup(n => n.Update(categoryDb)).Returns(categoryDb);
            _mapper.Setup(n => n.Map<CategoryDb>(categoryEdit)).Returns(categoryDb);
            _mapper.Setup(n => n.Map<Category>(categoryDb)).Returns(mappedCategory);

            var result = _sut.Save(categoryEdit);

            result.Should().BeEquivalentTo<Category>(new()
            {
                Id = -123,
                Name = "test name"
            });
        }

        [Fact]
        public void should_commit_changes()
        {
            _sut.Save(new());
            _categoriesUnitOfWork.Verify(n => n.Commit());
        }

        [Fact]
        public void should_throw_exception_when_category_equals_null()
        {
            _sut.Invoking(n => n.Save(null))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(ApiResponses.CategoryYouWishToUpdateCannotBeNull);
        }

        [Fact]
        public void should_throw_exception_when_cant_save_category()
        {
            _categories.Setup(n => n.Update(It.IsAny<CategoryDb>())).Throws(new Exception());
            _sut.Invoking(n => n.Save(new())).Should().Throw<Exception>();
        }
    }
}
