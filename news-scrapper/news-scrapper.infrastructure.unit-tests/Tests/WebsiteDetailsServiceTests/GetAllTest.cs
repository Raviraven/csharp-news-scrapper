using FluentAssertions;
using Moq;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models.WebsiteDetails;
using news_scrapper.infrastructure.unit_tests.Builders;
using System.Collections.Generic;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.WebsiteDetailsServiceTests
{
    public class GetAllTest : BaseTest
    {
        [Fact]
        public void should_return_all_website_details()
        {
            List<WebsiteDetails> websiteDetails = new WebsiteDetailsBuilder().Build(10);
            var gotWebsites = websiteDetails.Map();

            _mapper.Setup(n => n.Map<List<WebsiteDetails>>(It.IsAny<List<WebsiteDetailsDb>>())).Returns(websiteDetails);
            _websitesRepository.Setup(n => n.GetAll()).Returns(gotWebsites);

            var result = _sut.GetAll();

            result.Should().BeEquivalentTo(websiteDetails);
        }

        public static IEnumerable<object[]> noWebsitesFromDb =>
            new List<object[]>
            {
                new object[] { null },
                new object[] { new List<WebsiteDetails>() }
            };

        [Theory]
        [MemberData(nameof(noWebsitesFromDb))]
        public void should_return_empty_list_when_no_website_details_in_db(List<WebsiteDetails> websiteDetails)
        {
            var gotWebsites = websiteDetails.Map();

            _mapper.Setup(n => n.Map<List<WebsiteDetails>>(It.IsAny<List<WebsiteDetailsDb>>())).Returns(websiteDetails);
            _websitesRepository.Setup(n => n.GetAll()).Returns(gotWebsites);

            var result = _sut.GetAll();

            result.Should().BeEquivalentTo(websiteDetails);
        }
    }
}
