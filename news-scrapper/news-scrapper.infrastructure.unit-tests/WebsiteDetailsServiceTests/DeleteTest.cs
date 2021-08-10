using FluentAssertions;
using System;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.WebsiteDetailsServiceTests
{
    public class DeleteTest : BaseTest
    {
        [Fact]
        public void should_return_true()
        {
            int websiteId = 123;

            _websitesRepository.Setup(n => n.Delete(websiteId)).Returns(true);

            var result = _sut.Delete(websiteId);

            result.Should().BeTrue();
        }

        [Fact]
        public void should_call_repository_delete_website_details()
        {
            int websiteId = 123;
            bool websiteDeleted = false;
            Action<int> delete = (_) => { websiteDeleted = true; };

            _websitesRepository.Setup(n => n.Delete(websiteId)).Callback(delete);

            _sut.Delete(websiteId);

            websiteDeleted.Should().BeTrue();
        }
    }
}
