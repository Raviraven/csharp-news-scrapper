using FluentAssertions;
using Microsoft.Extensions.Options;
using news_scrapper.domain;
using news_scrapper.domain.Exceptions;
using news_scrapper.domain.Models;
using news_scrapper.infrastructure.Authorization;
using news_scrapper.infrastructure.unit_tests.Builders;
using System;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.AuthorizationTests
{
    public class GenerateJwtTokenTest : BaseTest
    {
        [Fact]
        public void should_generate_jwt_token()
        {
            var user = new UserBuilder().Build();
            DateTime now = DateTime.UtcNow.AddMonths(4);
            string expectedResult = generateToken(user, now, SECRET_KEY);

            _dateTimeProvider.Setup(n => n.Now).Returns(now);

            var result = _sut.GenerateJwtToken(user);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("wrongone")]
        public void should_throw_dev_exception_when_secret_key_too_short(string secret)
        {
            _appSettings = Options.Create(new AppSettings { Secret = secret });
            JwtUtils sut = new(_appSettings, _dateTimeProvider.Object, null);

            DateTime now = DateTime.UtcNow.AddMonths(4);
            User user = new UserBuilder().Build();

            _dateTimeProvider.Setup(n => n.Now).Returns(now);

            sut.Invoking(n => n.GenerateJwtToken(user))
                .Should()
                .Throw<DevException>()
                .WithMessage("Length of the secret key < 16");
        }
    }
}
