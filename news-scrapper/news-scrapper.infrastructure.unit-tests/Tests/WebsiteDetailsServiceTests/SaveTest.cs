using FluentAssertions;
using Moq;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Exceptions;
using news_scrapper.domain.Models.Categories;
using news_scrapper.domain.Models.WebsiteDetails;
using news_scrapper.infrastructure.unit_tests.Builders;
using news_scrapper.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.WebsiteDetailsServiceTests
{
    public class SaveTest : BaseTest
    {
        [Fact]
        public void should_save_website_details()
        {
            WebsiteDetails website = new WebsiteDetailsBuilder().Build();
            var websiteDb = website.Map();
            bool savedWebsite = false;

            Action<WebsiteDetailsDb> save = (_) => { savedWebsite = true; };

            _websitesRepository.Setup(n => n.GetWithCategories(website.Id)).Returns(websiteDb);
            _mapper.Setup(n => n.Map<WebsiteDetailsDb>(It.IsAny<WebsiteDetailsDb>())).Returns(websiteDb);
            _mapper.Setup(n => n.Map<WebsiteDetails>(It.IsAny<WebsiteDetailsDb>())).Returns(website);
            _websitesRepository.Setup(n => n.Save(It.IsAny<WebsiteDetailsDb>())).Callback(save);

            _sut.Save(website);

            savedWebsite.Should().BeTrue();
        }

        [Fact]
        public void should_return_saved_website_detail()
        {
            WebsiteDetails website = new WebsiteDetailsBuilder().WithId(123).Build();
            WebsiteDetailsDb savedWebsite = website.Map();

            _websitesRepository.Setup(n => n.GetWithCategories(website.Id)).Returns(savedWebsite);
            _mapper.Setup(n => n.Map<WebsiteDetailsDb>(It.IsAny<WebsiteDetailsDb>())).Returns(savedWebsite);
            _mapper.Setup(n => n.Map<WebsiteDetails>(It.IsAny<WebsiteDetailsDb>())).Returns(website);
            _websitesRepository.Setup(n => n.Save(It.IsAny<WebsiteDetailsDb>())).Returns(savedWebsite);

            var result = _sut.Save(website);

            result.Should().BeEquivalentTo(website);
        }

        [Fact]
        public void should_throw_exception_when_passing_null_website()
        {
            string errorMessage = ApiResponses.WebsiteDetailsCannotBeNull;
            WebsiteDetails website = null;
            WebsiteDetailsDb websiteDb = null;

            _mapper.Setup(n => n.Map<WebsiteDetailsDb>(It.IsAny<WebsiteDetailsDb>())).Returns(websiteDb);
            _mapper.Setup(n => n.Map<WebsiteDetails>(It.IsAny<WebsiteDetailsDb>())).Returns(website);

            _sut.Invoking(n => n.Save(website)).Should().Throw<InvalidWebsiteDetailsException>().WithMessage(errorMessage);
        }

        [Fact]
        public void should_throw_exception_when_trying_to_add_website_without_url()
        {
            string errorMessage = ApiResponses.WebsiteDetailsUrlCannotBeNullOrEmpty;
            WebsiteDetails website = new WebsiteDetailsBuilder().WithId(0).WithUrl(null).Build();
            var websiteDb = website.Map();

            _mapper.Setup(n => n.Map<WebsiteDetailsDb>(It.IsAny<WebsiteDetailsDb>())).Returns(websiteDb);
            _mapper.Setup(n => n.Map<WebsiteDetails>(It.IsAny<WebsiteDetailsDb>())).Returns(website);

            _sut.Invoking(n => n.Save(website)).Should().Throw<InvalidWebsiteDetailsException>().WithMessage(errorMessage);
        }

        [Fact]
        public void should_throw_exception_when_trying_to_add_website_without_main_node_xpath()
        {
            string errorMessage = ApiResponses.WebsiteDetailsXpathCannotBeNullOrEmpty;
            WebsiteDetails website = new WebsiteDetailsBuilder().WithId(0).Build();
            website.MainNodeXPathToNewsContainer = null;
            var websiteDb = website.Map();

            _mapper.Setup(n => n.Map<WebsiteDetailsDb>(It.IsAny<WebsiteDetailsDb>())).Returns(websiteDb);
            _mapper.Setup(n => n.Map<WebsiteDetails>(It.IsAny<WebsiteDetailsDb>())).Returns(website);

            _sut.Invoking(n => n.Save(website)).Should().Throw<InvalidWebsiteDetailsException>().WithMessage(errorMessage);
        }

        [Fact]
        public void should_throw_exception_when_trying_to_add_website_without_news_node_tag()
        {
            string errorMessage = ApiResponses.WebsiteDetailsNewsNodeCannotBeNullOrEmpty;
            WebsiteDetails website = new WebsiteDetailsBuilder().WithId(0).Build();
            website.NewsNodeTag = null;
            var websiteDb = website.Map();

            _mapper.Setup(n => n.Map<WebsiteDetailsDb>(It.IsAny<WebsiteDetailsDb>())).Returns(websiteDb);
            _mapper.Setup(n => n.Map<WebsiteDetails>(It.IsAny<WebsiteDetailsDb>())).Returns(website);

            _sut.Invoking(n => n.Save(website)).Should().Throw<InvalidWebsiteDetailsException>().WithMessage(errorMessage);
        }
        
        [Fact]
        public void should_throw_exception_when_trying_to_save_website_with_duplicated_category()
        {
            CategoryWebsiteDetails[] categories = new CategoryWebsiteDetails[]
            {
                new CategoryWebsiteDetails() { Id = 1, Name = "something 1"},
                new CategoryWebsiteDetails() { Id = 1, Name = "duplicated ID"}
            };

            WebsiteDetails websiteDetails = new WebsiteDetailsBuilder().WithCategories(categories).Build();

            _sut.Invoking(n => n.Save(websiteDetails))
                .Should()
                .Throw<InvalidWebsiteDetailsException>()
                .WithMessage(ApiResponses.WebsiteDetailsCategoriesCannotBeDuplicated);
        }

        [Fact]
        public void should_get_categories_by_id_from_database()
        {
            CategoryWebsiteDetails[] categories = new CategoryWebsiteDetails[]
            {
                new CategoryWebsiteDetails() { Id = 1, Name = "something 1"},
                new CategoryWebsiteDetails() { Id = 2, Name = "something 2"}
            };

            WebsiteDetailsDb alreadyExistingWebsiteDetails = new WebsiteDetailsBuilder().WithCategories(2).Build().Map();
            alreadyExistingWebsiteDetails.Categories = new List<CategoryDb>() 
            { 
                new() { Id = 3, Name = "test name" },
                new() { Id = 4, Name = "test name 2" },
            };

            WebsiteDetails websiteDetails = new WebsiteDetailsBuilder().WithCategories(categories).Build();
            var mappedWebsiteDetails = websiteDetails.Map();

            _websitesRepository.Setup(n=>n.GetWithCategories(websiteDetails.Id)).Returns(alreadyExistingWebsiteDetails);

            _sut.Save(websiteDetails);

            _categoriesRepository.Verify(n => n.Get(
               It.IsAny<Expression<Func<CategoryDb, bool>>>(),
               It.IsAny<Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>>(),
               It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void should_remove_categories_not_existing_in_model()
        {
            CategoryWebsiteDetails[] categories = new CategoryWebsiteDetails[]
            {
                new CategoryWebsiteDetails() { Id = 1, Name = "something 1"},
                new CategoryWebsiteDetails() { Id = 2, Name = "something 2"}
            };

            List<CategoryDb> categoriesDb = new()
            {
                new() { Id = 1, Name = "something 1" },
                new() { Id = 2, Name = "something 2" }
            };

            List<CategoryDb> actualCategories = new()
            {
                new() { Id = 3, Name = "test name" },
                new() { Id = 4, Name = "test name 2" },
            };


            WebsiteDetailsDb alreadyExistingWebsiteDetails = new WebsiteDetailsBuilder().WithCategories(2).Build().Map();
            alreadyExistingWebsiteDetails.Categories = actualCategories;
            
            WebsiteDetails websiteDetails = new WebsiteDetailsBuilder().WithCategories(categories).Build();
            var mappedWebsiteDetails = websiteDetails.Map();

            WebsiteDetailsDb result = null;
            Action<WebsiteDetailsDb> saveWebsiteDetails = (websiteDb) => { result = websiteDb; };

            _websitesRepository.Setup(n => n.GetWithCategories(websiteDetails.Id)).Returns(alreadyExistingWebsiteDetails);
            _websitesRepository.Setup(n => n.Save(It.IsAny<WebsiteDetailsDb>())).Callback(saveWebsiteDetails);
            _categoriesRepository.Setup(n => n.Get(It.IsAny<Expression<Func<CategoryDb, bool>>>(),
               It.IsAny<Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>>(),
               It.IsAny<string>())).Returns(categoriesDb);

            _sut.Save(websiteDetails);

            result.Categories
                .Should()
                .NotContain(new List<CategoryDb>()
                    {
                        new() { Id = 3, Name = "test name" },
                        new() { Id = 4, Name = "test name 2" },
                    });
        }

        [Fact]
        public void should_save_chosen_categories()
        {
            CategoryWebsiteDetails[] categories = new CategoryWebsiteDetails[]
           {
                new CategoryWebsiteDetails() { Id = 1, Name = "something 1"},
                new CategoryWebsiteDetails() { Id = 2, Name = "something 2"}
           };

            List<CategoryDb> categoriesDb = new()
            {
                new() { Id = 1, Name = "something 1" },
                new() { Id = 2, Name = "something 2" }
            };

            List<CategoryDb> actualCategories = new()
            {
                new() { Id = 3, Name = "test name" },
                new() { Id = 4, Name = "test name 2" },
            };


            WebsiteDetailsDb alreadyExistingWebsiteDetails = new WebsiteDetailsBuilder().WithCategories(2).Build().Map();
            alreadyExistingWebsiteDetails.Categories = actualCategories;

            WebsiteDetails websiteDetails = new WebsiteDetailsBuilder().WithCategories(categories).Build();
            var mappedWebsiteDetails = websiteDetails.Map();

            WebsiteDetailsDb result = null;
            Action<WebsiteDetailsDb> saveWebsiteDetails = (websiteDb) => { result = websiteDb; };

            _websitesRepository.Setup(n => n.GetWithCategories(websiteDetails.Id)).Returns(alreadyExistingWebsiteDetails);
            _websitesRepository.Setup(n => n.Save(It.IsAny<WebsiteDetailsDb>())).Callback(saveWebsiteDetails);
            _categoriesRepository.Setup(n => n.Get(It.IsAny<Expression<Func<CategoryDb, bool>>>(),
               It.IsAny<Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>>(),
               It.IsAny<string>())).Returns(categoriesDb);

            _sut.Save(websiteDetails);

            result.Categories
                .Should()
                .BeEquivalentTo(new List<CategoryDb>()
                    {
                        new() { Id = 1, Name = "something 1" },
                        new() { Id = 2, Name = "something 2" }
                    });
        }

    }
}
