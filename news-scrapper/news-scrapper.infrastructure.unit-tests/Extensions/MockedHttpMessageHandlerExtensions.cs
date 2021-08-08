using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace news_scrapper.infrastructure.unit_tests
{
    public static class MockedHttpMessageHandlerExtensions
    {
        public static void Setup(this Mock<HttpMessageHandler> mockedMessageHandler, HttpResponseMessage response)
        {
            mockedMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>(
                    "SendAsync", 
                    ItExpr.IsAny<HttpRequestMessage>(), 
                    ItExpr.IsAny<CancellationToken>()
                    )
               .ReturnsAsync(response);
        }

        public static void Setup(this Mock<HttpMessageHandler> mockedMessageHandler, Exception exception)
        {
            mockedMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                    )
               .Throws(exception);
        }
    }
}
