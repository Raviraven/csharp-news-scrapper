﻿using FluentAssertions;
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
    }
}
