using FluentAssertions;
using Moq;
using news_scrapper.domain.Models;
using System;
using System.Text;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.AuthorizationTests
{
    public class GenerateRefreshTokenTest : BaseTest
    {
        [Fact]
        public void should_generate_refresh_token()
        {
            DateTime now = DateTime.UtcNow.AddMonths(5);
            string ipAddress = "0.0.0.0";
            byte[] randomBytes = Encoding.ASCII.GetBytes("just the string");

            var expectedResut = new RefreshToken 
            { 
                Token = Convert.ToBase64String(randomBytes),
                Expires = now.AddDays(7),
                Created = now,
                CreatedByIp = ipAddress
            };

            _dateTimeProvider.Setup(n => n.Now).Returns(now);
            _randomCryptoBytesGenerator.Setup(n => n.Get(It.IsAny<int>())).Returns(randomBytes);

            var result = _sut.GenerateRefreshToken(ipAddress);

            result.Should().BeEquivalentTo(expectedResut);
        }
    }
}
