using FluentAssertions;
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
    public class GetTest : BaseTest
    {
        [Fact]
        public void should_return_website_details()
        {
            int id = 1321;
            WebsiteDetails website = new WebsiteDetailsBuilder().Build();

            _websitesRepository.Setup(n => n.Get(id)).Returns(website);

            var result = _sut.Get(id);

            result.Should().BeEquivalentTo(website);
        }

        [Fact]
        public void should_return_null_when_not_found()
        {
            int id = 1321;
            WebsiteDetails website = null;

            _websitesRepository.Setup(n => n.Get(id)).Returns(website);

            var result = _sut.Get(id);

            result.Should().BeNull();
        }
    }
}
