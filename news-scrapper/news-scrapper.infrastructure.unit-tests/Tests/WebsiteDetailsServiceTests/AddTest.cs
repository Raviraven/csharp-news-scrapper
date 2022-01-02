using FluentAssertions;
using Moq;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Exceptions;
using news_scrapper.domain.Models.Categories;
using news_scrapper.domain.Models.WebsiteDetails;
using news_scrapper.infrastructure.unit_tests.Builders;
using news_scrapper.resources;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.WebsiteDetailsServiceTests
{
    public class AddTest : BaseTest
    {
        [Fact]
        public void should_add_website_details()
        {
            WebsiteDetails website = new WebsiteDetailsBuilder().Build();
            WebsiteDetailsDb websiteMapped = website.Map();
            bool websiteAdded = false;

            Action<WebsiteDetailsDb> addWebsite = (website) => { websiteAdded = true; };

            _mapper.Setup(n => n.Map<WebsiteDetailsDb>(It.IsAny<WebsiteDetails>())).Returns(websiteMapped);
            _websitesRepository.Setup(n => n.Add(It.IsAny<WebsiteDetailsDb>())).Callback(addWebsite);

            _sut.Add(website);

            websiteAdded.Should().BeTrue();
        }

        [Fact]
        public void should_return_added_website_details()
        {
            WebsiteDetails website = new WebsiteDetailsBuilder().WithId(0).Build();
            WebsiteDetailsDb websiteMapped = website.Map();
            website.Id = 829;

            _mapper.Setup(n => n.Map<WebsiteDetailsDb>(It.IsAny<WebsiteDetails>())).Returns(websiteMapped);
            _mapper.Setup(n => n.Map<WebsiteDetails>(It.IsAny<WebsiteDetailsDb>())).Returns(website);

            _websitesRepository.Setup(n => n.Add(It.IsAny<WebsiteDetailsDb>())).Returns(websiteMapped);

            var result = _sut.Add(website);

            result.Should().BeEquivalentTo(website);
        }

        [Fact]
        public void should_throw_exception_when_passing_null_website()
        {
            string errorMessage = ApiResponses.WebsiteDetailsCannotBeNull;
            WebsiteDetails website = null;

            _sut.Invoking(n => n.Add(website)).Should().Throw<InvalidWebsiteDetailsException>().WithMessage(errorMessage);
        }

        [Fact]
        public void should_throw_exception_when_trying_to_add_website_without_url()
        {
            string errorMessage = ApiResponses.WebsiteDetailsUrlCannotBeNullOrEmpty;
            WebsiteDetails website = new WebsiteDetailsBuilder().WithId(0).WithUrl(null).Build();

            _sut.Invoking(n => n.Add(website)).Should().Throw<InvalidWebsiteDetailsException>().WithMessage(errorMessage);
        }

        [Fact]
        public void should_throw_exception_when_trying_to_add_website_without_main_node_xpath()
        {
            string errorMessage = ApiResponses.WebsiteDetailsXpathCannotBeNullOrEmpty;
            WebsiteDetails website = new WebsiteDetailsBuilder().WithId(0).Build();
            website.MainNodeXPathToNewsContainer = null;

            _sut.Invoking(n => n.Add(website)).Should().Throw<InvalidWebsiteDetailsException>().WithMessage(errorMessage);
        }

        [Fact]
        public void should_throw_exception_when_trying_to_add_website_without_news_node_tag()
        {
            string errorMessage = ApiResponses.WebsiteDetailsNewsNodeCannotBeNullOrEmpty;
            WebsiteDetails website = new WebsiteDetailsBuilder().WithId(0).Build();
            website.NewsNodeTag = null;

            _sut.Invoking(n => n.Add(website)).Should().Throw<InvalidWebsiteDetailsException>().WithMessage(errorMessage);
        }

        [Fact]
        public void should_throw_exception_when_trying_to_add_website_with_duplicated_category()
        {
            CategoryWebsiteDetails[] categories = new CategoryWebsiteDetails[]
            {
                new CategoryWebsiteDetails() { Id = 1, Name = "something 1"},
                new CategoryWebsiteDetails() { Id = 1, Name = "duplicated ID"}
            };

            WebsiteDetails websiteDetails = new WebsiteDetailsBuilder().WithCategories(categories).Build();
            
            _sut.Invoking(n=>n.Add(websiteDetails))
                .Should()
                .Throw<InvalidWebsiteDetailsException>()
                .WithMessage(ApiResponses.WebsiteDetailsCategoriesCannotBeDuplicated);
        }

        [Fact]
        public void should_add_website_details_without_categories_when_none_chosen()
        {
            WebsiteDetails websiteDetails = new WebsiteDetailsBuilder().WithCategories(0).Build();
            var mappedWebsiteDetails = websiteDetails.Map();

            WebsiteDetailsDb addingWebsiteCategory = null;
            Action<WebsiteDetailsDb> addWebsiteCategory = (cat) => { addingWebsiteCategory = cat; };

            _websitesRepository.Setup(n => n.Add(It.IsAny<WebsiteDetailsDb>()))
                .Callback(addWebsiteCategory);
            _mapper.Setup(mapper => mapper.Map<WebsiteDetailsDb>(websiteDetails)).Returns(mappedWebsiteDetails);

            _sut.Add(websiteDetails);

            addingWebsiteCategory.Categories
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void should_get_categories_passed_through_json_by_ids_from_db()
        {
            CategoryWebsiteDetails[] categories = new CategoryWebsiteDetails[]
            {
                new CategoryWebsiteDetails() { Id = 1, Name = "something 1"},
                new CategoryWebsiteDetails() { Id = 2, Name = "something 2"}
            };

            //List<int> expectedCategoriesIds = new() { 1, 2 };

            WebsiteDetails websiteDetails = new WebsiteDetailsBuilder().WithCategories(categories).Build();
            var mappedWebsiteDetails = websiteDetails.Map();

            //Expression<Func<CategoryDb, bool>> categoriesFilter = null;
            //Action<Expression<Func<CategoryDb, bool>>, Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>, string> getCategories
            //    = (filter, order, includeProps) => { categoriesFilter = filter; };

            _mapper.Setup(mapper => mapper.Map<WebsiteDetailsDb>(websiteDetails)).Returns(mappedWebsiteDetails);
            //_categoriesRepository.Setup(n => n.Get(It.IsAny<Expression<Func<CategoryDb, bool>>>(),
            //    It.IsAny<Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>>(),
            //    It.IsAny<string>())).Callback(getCategories);

            _sut.Add(websiteDetails);

            _categoriesRepository.Verify(n => n.Get(//n => categoriesIds.Contains(n.Id),
                It.IsAny<Expression<Func<CategoryDb, bool>>>(),
                It.IsAny<Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>>(),
                It.IsAny<string>()), Times.Once);
        }
    }
}
