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
    public class ValidateJwtTokenTest : BaseTest
    {
        [Fact]
        public void should_return_user_id()
        {
            User userForToken = new UserBuilder().Build();
            DateTime now = DateTime.UtcNow.AddMonths(5);

            var token = generateToken(userForToken, now, SECRET_KEY);

            var result = _sut.ValidateJwtToken(token);

            result.Should().Be(userForToken.Id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void should_return_null_when_token_is_null_or_empty(string token)
        {
            var result = _sut.ValidateJwtToken(token);
            result.Should().BeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("wrongone")]
        public void should_throw_dev_exception_when_wrong_secret_key(string secret)
        {
            _appSettings = Options.Create(new AppSettings { Secret = secret });
            JwtUtils sut = new(_appSettings, _dateTimeProvider.Object, _randomCryptoBytesGenerator.Object);

            sut.Invoking(n => n.ValidateJwtToken("token"))
                .Should()
                .Throw<DevException>()
                .WithMessage("Length of the secret key < 16");
        }

        [Fact]
        public void should_return_null_when_token_invalid()
        {
            User userForToken = new UserBuilder().Build();
            DateTime now = DateTime.UtcNow.AddMonths(5);
            string wrongKey = "just the key to generate wrong token";
            var token = generateToken(userForToken, now, wrongKey);

            var result =  _sut.ValidateJwtToken(token);

            result.Should().BeNull();
        }
    }
}
