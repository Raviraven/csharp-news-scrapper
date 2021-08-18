using FluentAssertions;
using Moq;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models;
using news_scrapper.infrastructure.unit_tests.Builders;
using news_scrapper.resources;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.UserServiceTests
{
    public class RevokeTokenTest : BaseTest
    {
        [Fact]
        public void should_revoke_token()
        {
            RefreshToken activeRefreshToken = new RefreshTokenBuilder().Build();
            List<RefreshToken> refreshTokens = new() { activeRefreshToken };

            User user = new UserBuilder().WithRefreshTokens(refreshTokens).Build();
            UserDb userFromDb = user.Map();
            List<UserDb> usersFromDb = new() { userFromDb };

            User userUpdated = null;
            Action<object, object> mapFinal = (a, b) => { userUpdated = a as User; };

            DateTime now = DateTime.UtcNow;
            string reason = ApiResponses.RevokedWithoutReplacement;
            var expectedResult = new List<RefreshToken> { getCopiedRevokedToken(activeRefreshToken, now, reason) };

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, ""))
                .Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(userFromDb)).Returns(user);
            _mapper.Setup(n => n.Map(It.IsAny<User>(), It.IsAny<UserDb>())).Callback(mapFinal);
            _dateTimeProvider.Setup(n => n.Now).Returns(now);

            _sut.RevokeToken(activeRefreshToken.Token, IP_ADDRESS);

            userUpdated.RefreshTokens.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void should_throw_exception_when_user_not_found()
        {
            List<UserDb> usersFromDb = new() { };

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, ""))
                .Returns(usersFromDb);

            _sut.Invoking(n => n.RevokeToken("test token", IP_ADDRESS))
                .Should()
                .Throw<Exception>()
                .WithMessage(ApiResponses.InvalidToken);
        }

        [Fact]
        public void should_throw_exception_when_token_is_inactive()
        {
            RefreshToken inactiveRefreshToken = new RefreshTokenBuilder().Inactive().Build();
            List<RefreshToken> refreshTokens = new() { inactiveRefreshToken };

            User user = new UserBuilder().WithRefreshTokens(refreshTokens).Build();
            UserDb userFromDb = user.Map();
            List<UserDb> usersFromDb = new() { userFromDb };

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, ""))
                .Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(userFromDb)).Returns(user);

            _sut.Invoking(n => n.RevokeToken(inactiveRefreshToken.Token, IP_ADDRESS))
                .Should()
                .Throw<Exception>()
                .WithMessage(ApiResponses.InvalidToken);
        }


        private RefreshToken getCopiedRevokedToken(RefreshToken token, DateTime now, string reason)
        {
            var result = token.GetCopy();
            result.Revoked = now;
            result.RevokedByIp = IP_ADDRESS;
            result.ReasonRevoked = reason;
            result.ReplacedByToken = null;
            return result;
        }
    }
}
