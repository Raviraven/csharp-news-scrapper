using FluentAssertions;
using Moq;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models;
using news_scrapper.domain.Models.UsersAuth;
using news_scrapper.infrastructure.unit_tests.Builders;
using news_scrapper.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.UserServiceTests
{
    public class RefreshTokenTest : BaseTest
    {

        [Fact]
        public void should_return_new_jwt_and_refresh_token()
        {
            List<RefreshToken> tokens = new() { new RefreshTokenBuilder().Build() };
            User user = new UserBuilder().WithRefreshTokens(tokens).Build();
            UserDb userFromDb = user.Map();
            var usersFromDb = new List<UserDb> { userFromDb };
            DateTime now = DateTime.UtcNow;

            var newRefreshToken = new RefreshTokenBuilder().Build();
            string newJwtToken = "new jwt token";
            AuthenticateResponse expectedResult = new(user, newJwtToken, newRefreshToken.Token);

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, "")).Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(userFromDb)).Returns(user);
            _dateTimeProvider.Setup(n => n.Now).Returns(now);
            _jwtUtils.Setup(n => n.GenerateRefreshToken(IP_ADDRESS)).Returns(newRefreshToken);
            _jwtUtils.Setup(n => n.GenerateJwtToken(user)).Returns(newJwtToken);

            var result = _sut.RefreshToken(tokens.First().Token, IP_ADDRESS);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void should_throw_exception_when_user_not_found()
        {
            var usersFromDb = new List<UserDb>();

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, "")).Returns(usersFromDb);

            _sut.Invoking(n => n.RefreshToken("test token", IP_ADDRESS))
                .Should()
                .Throw<Exception>()
                .WithMessage(ApiResponses.InvalidToken);
        }


        //[0] - first inactive token replaced by [1]
        //[1] - second inactive token replaced by [2]
        //[2] - active token
        //user is trying to use [0] token 
        //etc.
        [Theory]
        [InlineData(2, 1)]
        [InlineData(3, 1)]
        [InlineData(3, 2)]
        public void should_revoke_descendant_token_when_current_token_is_revoked(
            int numberOfTokens, int whichTokenToUse)
        {
            int tokenIndexToUse = whichTokenToUse - 1;
            List<RefreshToken> tokens = generateFakeReplacedTokensChain(numberOfTokens);
            User user = new UserBuilder().WithRefreshTokens(tokens).Build();
            UserDb userFromDb = user.Map();
            var usersFromDb = new List<UserDb> { userFromDb };

            DateTime now = DateTime.UtcNow;
            string reason = string.Format(ApiResponses.AttemptedReuseOfRevokedToken, tokens[tokenIndexToUse].Token);
            RefreshToken expectedResult = getLastTokenRevoked(tokens, now, reason);

            User userWithRevokedTokens = null;
            Action<object> mapUserWithRevokedTokens = (usr) => { userWithRevokedTokens = usr as User; };

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, "")).Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(userFromDb)).Returns(user);
            _mapper.Setup(n => n.Map<UserDb>(It.IsAny<User>())).Callback(mapUserWithRevokedTokens);
            _dateTimeProvider.Setup(n => n.Now).Returns(now);


            _sut.Invoking(n => n.RefreshToken(tokens[tokenIndexToUse].Token, IP_ADDRESS)).Should().Throw<Exception>();

            userWithRevokedTokens.RefreshTokens.Last().Should().BeEquivalentTo(expectedResult);
        }


        //[0] - first inactive token replaced by [1]
        //[1] - second inactive token replaced by [2]
        //[2] - third inactive token not replaced. - nothing happens... 
        //user is trying to use [0] token 
        [Theory]
        [InlineData(2, 1)]
        [InlineData(3, 1)]
        [InlineData(3, 2)]
        [InlineData(3, 3)]
        public void should_not_change_any_tokens_when_theyre_already_inactive(int numberOfTokens, int whichTokenToUse)
        {
            int tokenIndexToUse = whichTokenToUse - 1;
            List<RefreshToken> tokens = generateFakeReplacedTokensChain(numberOfTokens);
            tokens.Last().Revoked = DateTime.UtcNow;

            User user = new UserBuilder().WithRefreshTokens(tokens).Build();
            UserDb userFromDb = user.Map();
            var usersFromDb = new List<UserDb> { userFromDb };
            DateTime now = DateTime.UtcNow;

            User userWithRevokedTokens = null;
            Action<object> mapUserWithRevokedTokens = (usr) => { userWithRevokedTokens = usr as User; };

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, "")).Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(userFromDb)).Returns(user);
            _mapper.Setup(n => n.Map<UserDb>(It.IsAny<User>())).Callback(mapUserWithRevokedTokens);
            _dateTimeProvider.Setup(n => n.Now).Returns(now);


            _sut.Invoking(n => n.RefreshToken(tokens[tokenIndexToUse].Token, IP_ADDRESS)).Should().Throw<Exception>();

            userWithRevokedTokens.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void should_throw_exception_when_current_token_is_inactive()
        {
            List<RefreshToken> tokens = new() { new RefreshTokenBuilder().Inactive().Build() };
            User user = new UserBuilder().WithRefreshTokens(tokens).Build();
            UserDb userFromDb = user.Map();
            var usersFromDb = new List<UserDb> { userFromDb };
            DateTime now = DateTime.UtcNow;

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, "")).Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(userFromDb)).Returns(user);
            _dateTimeProvider.Setup(n => n.Now).Returns(now);

            _sut.Invoking(n => n.RefreshToken(tokens.Last().Token, IP_ADDRESS))
                .Should()
                .Throw<Exception>()
                .WithMessage(ApiResponses.InvalidToken);
        }


        [Fact]
        public void should_call_generate_new_refresh_token()
        {
            List<RefreshToken> tokens = new() { new RefreshTokenBuilder().Build() };
            User user = new UserBuilder().WithRefreshTokens(tokens).Build();
            UserDb userFromDb = user.Map();
            var usersFromDb = new List<UserDb> { userFromDb };
            DateTime now = DateTime.UtcNow;

            bool generatedRefreshToken = false;
            Action<string> generateRefreshtoken = (ip) => { generatedRefreshToken = true; };

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, "")).Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(userFromDb)).Returns(user);
            _dateTimeProvider.Setup(n => n.Now).Returns(now);
            _jwtUtils.Setup(n => n.GenerateRefreshToken(IP_ADDRESS)).Callback(generateRefreshtoken);


            _sut.Invoking(n=>n.RefreshToken(tokens.Last().Token, IP_ADDRESS)).Should().Throw<Exception>();
            generatedRefreshToken.Should().BeTrue();
        }

        [Fact]
        public void should_add_new_refresh_token()
        {
            List<RefreshToken> tokens = new() { new RefreshTokenBuilder().Build() };
            User user = new UserBuilder().WithRefreshTokens(tokens).Build();
            UserDb userFromDb = user.Map();
            var usersFromDb = new List<UserDb> { userFromDb };
            DateTime now = DateTime.UtcNow;

            var newRefreshToken = new RefreshTokenBuilder().Build();

            User result = null;
            Action<User, UserDb> mapFinalUser = (a, b) => { result = a; };

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, "")).Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(userFromDb)).Returns(user);
            _dateTimeProvider.Setup(n => n.Now).Returns(now);
            _jwtUtils.Setup(n => n.GenerateRefreshToken(IP_ADDRESS)).Returns(newRefreshToken);
            _mapper.Setup(n => n.Map(user, userFromDb)).Callback(mapFinalUser);

            _sut.RefreshToken(tokens.First().Token, IP_ADDRESS);

            result.RefreshTokens.Should().Contain(newRefreshToken);
        }

        [Fact]
        public void should_revoke_old_tokens_when_add_new_refresh_token()
        {
            List<RefreshToken> tokens = new() { new RefreshTokenBuilder().Build() };
            User user = new UserBuilder().WithRefreshTokens(tokens).Build();
            UserDb userFromDb = user.Map();
            var usersFromDb = new List<UserDb> { userFromDb };
            DateTime now = DateTime.UtcNow;

            string reason = ApiResponses.ReplacedByNewToken;

            var newRefreshToken = new RefreshTokenBuilder().Build();
            var lastTokenRevoked = getLastTokenRevoked(tokens, now, reason, newRefreshToken.Token);

            List<RefreshToken> expectedRefreshTokens = new() { lastTokenRevoked, newRefreshToken };

            User result = null;
            Action<User, UserDb> mapFinalUser = (a, b) => { result = a; };

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, "")).Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(userFromDb)).Returns(user);
            _dateTimeProvider.Setup(n => n.Now).Returns(now);
            _jwtUtils.Setup(n => n.GenerateRefreshToken(IP_ADDRESS)).Returns(newRefreshToken);
            _mapper.Setup(n => n.Map(user, userFromDb)).Callback(mapFinalUser);

            _sut.RefreshToken(tokens.First().Token, IP_ADDRESS);

            result.RefreshTokens.Should().BeEquivalentTo(expectedRefreshTokens);
        }

        [Fact]
        public void should_remove_old_refresh_tokens()
        {
            List<RefreshToken> tokens = generateFakeReplacedTokensChain(2); // new RefreshTokenBuilder().Build(2);
            User user = new UserBuilder().WithRefreshTokens(tokens).Build();
            UserDb userFromDb = user.Map();
            DateTime now = new DateTime(2022, 05, 05);
            string reason = ApiResponses.ReplacedByNewToken;
            var usersFromDb = new List<UserDb> { userFromDb };

            tokens[0].Created = new DateTime(2022, 01, 01);
            tokens[0].Revoked = new DateTime(2022, 01, 02);
            tokens[1].Created = new DateTime(2022, 05, 05);

            var newRefreshToken = new RefreshTokenBuilder().WithCreated(new DateTime(2022, 05, 10)).Build();
            var lastTokenRevoked = getLastTokenRevoked(tokens, now, reason, newRefreshToken.Token);

            var expectedTokens = new List<RefreshToken> { lastTokenRevoked, newRefreshToken };

            User result = null;
            Action<User, UserDb> mapFinalUser = (a, b) => { result = a; };
            
            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, "")).Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(userFromDb)).Returns(user);
            _dateTimeProvider.Setup(n => n.Now).Returns(now);
            _jwtUtils.Setup(n => n.GenerateRefreshToken(IP_ADDRESS)).Returns(newRefreshToken);
            _mapper.Setup(n => n.Map(user, userFromDb)).Callback(mapFinalUser);

            _sut.RefreshToken(tokens[1].Token, IP_ADDRESS);

            result.RefreshTokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Fact]
        public void should_commit()
        {
            List<RefreshToken> tokens = new() { new RefreshTokenBuilder().Build() };
            User user = new UserBuilder().WithRefreshTokens(tokens).Build();
            UserDb userFromDb = user.Map();
            var usersFromDb = new List<UserDb> { userFromDb };
            DateTime now = DateTime.UtcNow;

            var newRefreshToken = new RefreshTokenBuilder().Build();

            bool commited = false;
            Action commit = () => { commited = true; };

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, "")).Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(userFromDb)).Returns(user);
            _dateTimeProvider.Setup(n => n.Now).Returns(now);
            _jwtUtils.Setup(n => n.GenerateRefreshToken(IP_ADDRESS)).Returns(newRefreshToken);
            _usersUnitOfWork.Setup(n => n.Commit()).Callback(commit);

            _sut.RefreshToken(tokens.First().Token, IP_ADDRESS);

            commited.Should().BeTrue();
        }

        [Fact]
        public void should_call_generate_new_jwt_token()
        {
            List<RefreshToken> tokens = new() { new RefreshTokenBuilder().Build() };
            User user = new UserBuilder().WithRefreshTokens(tokens).Build();
            UserDb userFromDb = user.Map();
            var usersFromDb = new List<UserDb> { userFromDb };
            DateTime now = DateTime.UtcNow;

            var newRefreshToken = new RefreshTokenBuilder().Build();

            bool newJwtTokenGenerated = false;
            Action<User> generateJwtToken = (usr) => { newJwtTokenGenerated = true; };

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, "")).Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(userFromDb)).Returns(user);
            _dateTimeProvider.Setup(n => n.Now).Returns(now);
            _jwtUtils.Setup(n => n.GenerateRefreshToken(IP_ADDRESS)).Returns(newRefreshToken);
            _jwtUtils.Setup(n => n.GenerateJwtToken(user)).Callback(generateJwtToken);

            _sut.RefreshToken(tokens.First().Token, IP_ADDRESS);

            newJwtTokenGenerated.Should().BeTrue();
        }


        private static RefreshToken getLastTokenRevoked(List<RefreshToken> tokens, DateTime now, string reason, string replacedByToken = null)
        {
            var expectedResult = tokens.Last().GetCopy();
            expectedResult.Revoked = now;
            expectedResult.RevokedByIp = IP_ADDRESS;
            expectedResult.ReasonRevoked = reason;
            expectedResult.ReplacedByToken = replacedByToken;
            return expectedResult;
        }

        private static List<RefreshToken> generateFakeReplacedTokensChain(int count)
        {
            var tokens = new RefreshTokenBuilder().Build(count);

            for (int i = 0; i < count - 1; i++)
            {
                tokens[i].ReplacedByToken = tokens[i + 1].Token;
                tokens[i].Expires = DateTime.UtcNow.AddMonths(5);
                tokens[i].Revoked = DateTime.UtcNow.Subtract(TimeSpan.FromDays(150));
            }

            return tokens;
        }

    }
}
