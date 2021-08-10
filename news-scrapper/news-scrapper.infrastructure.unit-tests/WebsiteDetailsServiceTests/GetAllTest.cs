using Bogus;
using FluentAssertions;
using Moq;
using news_scrapper.domain;
using news_scrapper.infrastructure.unit_tests.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.WebsiteDetailsServiceTests
{
    public class GetAllTest : BaseTest
    {
        [Fact]
        public async void should_return_all_website_details()
        {
            List<WebsiteDetails> websiteDetails = new WebsiteDetailsBuilder().Build(10);

            _websitesRepository.Setup(n => n.GetAll()).ReturnsAsync(websiteDetails);

            var result = await _sut.GetAll();

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
        public async void should_return_empty_list_when_no_website_details_in_db(List<WebsiteDetails> websiteDetails)
        {
            _websitesRepository.Setup(n => n.GetAll()).ReturnsAsync(websiteDetails);

            var result = await _sut.GetAll();

            result.Should().BeEquivalentTo(websiteDetails);
        }
    }
}
