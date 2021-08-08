using FluentAssertions;
using Moq;
using news_scrapper.resources;
using System;
using System.Net.Http;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.WebsiteServiceTests
{
    public class GetRawHtmlTest
    {
        protected HttpClient _httpClient { get; set; }
        protected Mock<HttpMessageHandler> _httpMessageHandler { get; set; }

        protected WebsiteService _sut { get; set; }

        public GetRawHtmlTest()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandler.Object);

            _sut = new WebsiteService(_httpClient);
        }

        [Fact]
        public async void should_return_raw_html()
        {
            string url = "http://test.test";
            string rawHtml = "<html></html>";
            HttpResponseMessage response = new() { Content = new StringContent(rawHtml) };

            _httpMessageHandler.Setup(response);

            var result = await _sut.GetRawHtml(url);

            result.Should().Be(rawHtml);
        }

        [Fact]
        public async void should_return_exception_message_when_cant_reach_url()
        {
            string url = "http://test.test";
            string exceptionMessage = string.Format(ApiResponses.CannotReachSiteWithGivenUrl, url);

            _httpMessageHandler.Setup(new HttpRequestException(exceptionMessage));

            var result = await _sut.GetRawHtml(url);

            result.Should().Be(exceptionMessage);
        }

        [Fact]
        public async void should_return_exception_message_when_unexpected_error_occured()
        {
            string url = "http://test.test";
            string exceptionMessage = "any unexpected error message";
            string excpectedMessage = string.Format(ApiResponses.UnexpectedErrorOccured, exceptionMessage);

            _httpMessageHandler.Setup(new Exception(exceptionMessage));

            var result = await _sut.GetRawHtml(url);

            result.Should().Be(excpectedMessage);
        }
    }
}
