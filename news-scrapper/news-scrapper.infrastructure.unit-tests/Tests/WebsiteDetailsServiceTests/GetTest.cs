using FluentAssertions;
using Moq;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models.WebsiteDetails;
using news_scrapper.infrastructure.unit_tests.Builders;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.WebsiteDetailsServiceTests
{
    public class GetTest : BaseTest
    {
        [Fact]
        public void should_return_website_details()
        {
            int id = 1321;
            WebsiteDetails website = new WebsiteDetailsBuilder().Build();
            WebsiteDetailsDb gotWebsite = website.Map();

            _mapper.Setup(n => n.Map<WebsiteDetails>(It.IsAny<WebsiteDetailsDb>())).Returns(website);
            _websitesRepository.Setup(n => n.Get(id)).Returns(gotWebsite);

            var result = _sut.Get(id);

            result.Should().BeEquivalentTo(website);
        }

        [Fact]
        public void should_return_null_when_not_found()
        {
            int id = 1321;
            WebsiteDetails website = null;
            WebsiteDetailsDb gotWebsite = website.Map();

            _mapper.Setup(n => n.Map<WebsiteDetails>(It.IsAny<WebsiteDetailsDb>())).Returns(website);
            _websitesRepository.Setup(n => n.Get(id)).Returns(gotWebsite);

            var result = _sut.Get(id);

            result.Should().BeNull();
        }
    }
}
