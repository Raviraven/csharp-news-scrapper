using FluentAssertions;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models.Categories;
using news_scrapper.infrastructure.unit_tests.Extensions;
using news_scrapper.resources;
using System;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.CategoriesServiceTests
{
    public class AddTest : BaseTest
    {
        [Fact]
        public void should_add_category()
        {
            CategoryAdd category = new() { Name = "Category test name" };
            CategoryDb mappedCategory = category.Map();

            _mapper.Setup(n => n.Map<CategoryDb>(category)).Returns(mappedCategory);

            _sut.Add(category);

            _categories.Verify(n=>n.Insert(mappedCategory));
        }

        [Fact]
        public void should_commit_db_changes()
        {
            CategoryAdd category = new() { Name = "Category test name" };
            CategoryDb mappedCategory = category.Map();

            _mapper.Setup(n => n.Map<CategoryDb>(category)).Returns(mappedCategory);

            _sut.Add(category);

            _categoriesUnitOfWork.Verify(n => n.Commit());
        }

        [Fact]
        public void should_throw_exception_when_category_is_null()
        {
            CategoryAdd category = null;

            _sut.Invoking(n => n.Add(category))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(ApiResponses.CannotAddNullCategory);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void should_throw_exception_when_name_is_null_or_empty(string categoryName)
        {
            CategoryAdd category = new() { Name = categoryName };

            _sut.Invoking(n => n.Add(category))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(ApiResponses.CannotAddCategoryWithNameNullOrEmpty);
        }
    }
}
