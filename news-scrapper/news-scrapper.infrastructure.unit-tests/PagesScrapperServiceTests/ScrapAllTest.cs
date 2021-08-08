using Bogus;
using FluentAssertions;
using Moq;
using news_scrapper.application.Interfaces;
using news_scrapper.application.Repositories;
using news_scrapper.domain;
using news_scrapper.domain.ResponseViewModels;
using news_scrapper.infrastructure.Data;
using news_scrapper.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.PagesScrapperServiceTests
{
    public class ScrapAllTest
    {
        private Mock<IHtmlScrapper> _htmlScrapper { get; set; }
        private Mock<IWebsiteService> _websiteService { get; set; }
        private Mock<IWebsitesRepository> _websiteRepository { get; set; }

        protected PagesScrapperService _sut { get; set; }

        public ScrapAllTest()
        {
            _htmlScrapper = new Mock<IHtmlScrapper>();
            _websiteService = new Mock<IWebsiteService>();
            _websiteRepository = new Mock<IWebsitesRepository>();

            _sut = new PagesScrapperService(_htmlScrapper.Object,
                _websiteService.Object, 
                _websiteRepository.Object);
        }

        [Fact]
        public async void should_return_articles()
        {
            var websites = new Faker<WebsiteDetails>()
                .RuleFor(n=>n.Url, b=>b.Name.FirstName())
                .Generate(3);

            string rawHtmlMocked = "<testtag>";

            var articles = new Faker<Article>()
                .RuleFor(n=>n.Url, b => b.Internet.Url())
                .RuleFor(n=>n.Title, b=>b.Name.FirstName())
                .RuleFor(n=>n.ImageUrl, b=>b.Internet.Url())
                .RuleFor(n=>n.Description, b=>b.Name.LastName())
                .Generate();

            ArticlesResponseViewModel expectedResult = new()
            {
                Articles = new() { articles, articles, articles },
                ErrorMessages = new()
            };

            _websiteRepository.Setup(n => n.GetAll()).ReturnsAsync(websites);
            _websiteService.Setup(n => n.GetRawHtml(It.IsAny<string>())).ReturnsAsync(rawHtmlMocked);
            _htmlScrapper.Setup(n => n.Scrap(It.IsAny<WebsiteDetails>(), rawHtmlMocked))
                .ReturnsAsync(new List<Article> { articles });

            var result = await _sut.ScrapAll();

            result.Should().BeEquivalentTo(expectedResult);
        }

        public static IEnumerable<object[]> EmptyWebsites =>
            new List<object[]>
            {
                new object[] { null },
                new object[] { new List<WebsiteDetails>() }
            };

        [Theory]
        [MemberData(nameof(EmptyWebsites))]
        public async void should_return_empty_list_when_no_websites_in_db(List<WebsiteDetails> websites)
        {
            _websiteRepository.Setup(n => n.GetAll()).ReturnsAsync(websites);

            var result = await _sut.ScrapAll();

            result.Articles.Should().BeEmpty();
        }

        [Theory]
        [MemberData(nameof(EmptyWebsites))]
        public async void should_return_errors_when_no_websites_in_db(List<WebsiteDetails> websites)
        {
            ArticlesResponseViewModel expectedResult = new() { 
                ErrorMessages = new List<string> { ApiResponses.ThereAreNoWebsitesToScrap }
            };

            _websiteRepository.Setup(n => n.GetAll()).ReturnsAsync(websites);

            var result = await _sut.ScrapAll();

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void should_return_empty_articles_and_error_messages_when_wrong_raw_html()
        {
            var websites = new Faker<WebsiteDetails>()
               .RuleFor(n => n.Url, b => b.Name.FirstName())
               .Generate(3);

            string rawHtmlMocked = "wrong raw html";


            ArticlesResponseViewModel expectedResult = new() {
                Articles = new(),
                ErrorMessages = new() { rawHtmlMocked, rawHtmlMocked, rawHtmlMocked }
            };

           
            _websiteRepository.Setup(n => n.GetAll()).ReturnsAsync(websites);
            _websiteService.Setup(n => n.GetRawHtml(It.IsAny<string>())).ReturnsAsync(rawHtmlMocked);

            var result = await _sut.ScrapAll();

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void should_return_articles_from_raw_html()
        {

        }
    }
}
