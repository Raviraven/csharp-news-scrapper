using FluentAssertions;
using Moq;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models.Categories;
using news_scrapper.infrastructure.unit_tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.CategoriesServiceTests
{
    public class GetTest : BaseTest
    {
        [Fact]
        public void should_return_list_of_categories()
        {
            List<CategoryDb> categories = new() 
            { 
                new() { Id = 1, Name = "name 1"},
                new() { Id = 2, Name = "name 2" },
            };

            var mappedCategories = categories.MapToCategory();

            _categories.Setup(n => n.Get(It.IsAny<Expression<Func<CategoryDb, bool>>>(),
                It.IsAny<Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>>(),
                It.IsAny<string>())).Returns(categories);
            _mapper.Setup(n=>n.Map<List<Category>>(categories)).Returns(mappedCategories);

            var result = _sut.Get();

            result.Should().BeEquivalentTo(new List<Category> { 
                new() { Id = 1, Name = "name 1" },
                new() { Id = 2, Name = "name 2" }
            });
        }

        [Fact]
        public void should_return_empty_list_when_no_categories_in_db()
        {
            List<CategoryDb> categoriesDb = new();
            var mappedCategories = categoriesDb.MapToCategory();

            _categories.Setup(n => n.Get(It.IsAny<Expression<Func<CategoryDb, bool>>>(),
                It.IsAny<Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>>(),
                It.IsAny<string>())).Returns(categoriesDb);
            _mapper.Setup(n => n.Map<List<Category>>(categoriesDb)).Returns(mappedCategories);

            var result = _sut.Get();

            result.Should().BeEmpty(); 
        }

        [Fact]
        public void should_return_single_category_with_websites_included_by_id()
        {
            CategoryDb categoryDb = new()
            {
                Id = 1, Name = "test category name", Websites = new List<WebsiteDetailsDb>() { }
            };
            Category categoryFound =  categoryDb.MapToCategory();

            _categories.Setup(n=>n.GetById(1, "Websites")).Returns(categoryDb);
            _mapper.Setup(n => n.Map<Category>(categoryDb)).Returns(categoryFound);

            var result = _sut.Get(1);

            result.Should().BeEquivalentTo(categoryFound);
        }

        [Fact]
        public void should_reutrn_null_when_category_by_id_not_found()
        {
            CategoryDb categoryDb = null;

            _categories.Setup(n => n.GetById(1)).Returns(categoryDb);

            var result = _sut.Get(1);

            Assert.Null(result);
        }

        [Fact]
        public void should_return_single_category_by_name()
        {
            List<CategoryDb> categoriesDb = new()
            {
                new()
                {
                    Id = 1,
                    Name = "test name",
                    Websites = new List<WebsiteDetailsDb>() { }
                }
            };

            var categoryFound = categoriesDb[0].MapToCategory();

            _categories.Setup(n => n.Get(
                n => n.Name == "name",
                It.IsAny<Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>>(),
                It.IsAny<string>()
                )).Returns(categoriesDb);

            _mapper.Setup(n => n.Map<Category>(categoriesDb[0])).Returns(categoryFound);

            var result = _sut.Get("name");

            result.Should().BeEquivalentTo(categoryFound);
        }

        [Fact]
        public void should_reutrn_null_when_category_by_name_not_found()
        {
            List<CategoryDb> categoriesDb = new();

            _categories.Setup(n => n.Get(
                n => n.Name == "name",
                It.IsAny<Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>>(),
                It.IsAny<string>()
                )).Returns(categoriesDb);

            var result = _sut.Get("name");

            Assert.Null(result);
        }
    }
}
