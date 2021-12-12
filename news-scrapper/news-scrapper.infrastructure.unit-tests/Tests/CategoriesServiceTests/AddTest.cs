using FluentAssertions;
using Moq;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models.Categories;
using news_scrapper.infrastructure.unit_tests.Extensions;
using news_scrapper.resources;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void should_insert_category_with_empty_websites_when_WebsiteIds_not_fulfilled()
        {
            CategoryAdd category = new() { Name = "Category test name" };
            CategoryDb mappedCategory = category.Map();

            CategoryDb insertedCategoryDb = null;
            Action<CategoryDb> insertCategory = (cat) => { insertedCategoryDb = cat; };

            _mapper.Setup(n => n.Map<CategoryDb>(category)).Returns(mappedCategory);
            _categories.Setup(n=>n.Insert(mappedCategory)).Callback(insertCategory);

            _sut.Add(category);

            insertedCategoryDb
                .Should()
                .BeEquivalentTo(
                    new CategoryDb { 
                       Id = 0,
                       Name = "Category test name",
                       Websites = null
                    }
                );
        }

        [Fact]
        public void should_add_website_details_when_WebsiteIds_fulfilled()
        {
            CategoryAdd category = new() { 
                Name = "Category test name", 
                WebsitesIds = new List<int> { 1, 3 }
            };
            CategoryDb mappedCategory = category.Map();

            WebsiteDetailsDb[] websiteDetails = new WebsiteDetailsDb[] { 
                new WebsiteDetailsDb { id = 1, Url = "test url 1" },
                new WebsiteDetailsDb { id = 3, Url = "test url 3" },
            };

            CategoryDb expectedResult = new()
            {
                Id = 0,
                Name = "Category test name",
                Websites = new List<WebsiteDetailsDb> {
                    new WebsiteDetailsDb { id = 1, Url = "test url 1" },
                    new WebsiteDetailsDb { id = 3, Url = "test url 3" }
                }
            };

            CategoryDb insertedCategoryDb = null;
            Action<CategoryDb> insertCategory = (cat) => { insertedCategoryDb = cat; };

            _mapper.Setup(n => n.Map<CategoryDb>(category)).Returns(mappedCategory);
            _websiteDetails.Setup(n => n.Get(
                n => category.WebsitesIds.Contains(n.id),
                It.IsAny<Func<IQueryable<WebsiteDetailsDb>, IOrderedQueryable<WebsiteDetailsDb>>>(),
                It.IsAny<string>())).Returns(websiteDetails);
            _categories.Setup(n => n.Insert(It.IsAny<CategoryDb>())).Callback(insertCategory);

            var result = _sut.Add(category);

            insertedCategoryDb.Should().BeEquivalentTo(expectedResult);
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
