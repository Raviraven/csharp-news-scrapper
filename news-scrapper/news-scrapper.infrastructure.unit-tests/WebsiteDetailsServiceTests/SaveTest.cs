using FluentAssertions;
using Moq;
using news_scrapper.domain;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Exceptions;
using news_scrapper.infrastructure.unit_tests.Builders;
using news_scrapper.resources;
using System;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.WebsiteDetailsServiceTests
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
    }
}
