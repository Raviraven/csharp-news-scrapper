using Bogus;
using FluentAssertions;
using Moq;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models;
using news_scrapper.domain.Models.UsersAuth;
using news_scrapper.infrastructure.unit_tests.Builders;
using news_scrapper.resources;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.UserServiceTests
{
    public class AuthenticateTest : BaseTest
    {
        private const string GENERATED_JWT_TOKEN = "generated test jwt token";

        [Fact]
        public void should_return_authenticate_response()
        {
            AuthenticateRequest request = createFakeRequest();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            User user = new UserBuilder().WithUsername(request.Username).WithPasswordHash(hashedPassword).Build();
            List<UserDb> usersFromDb = new() { user.Map() };

            DateTime now = DateTime.UtcNow;
            DateTime dateValid = now.AddDays(_appSettings.Value.RefreshTokenTTL + 5);
            RefreshToken generatedRefreshToken = new RefreshTokenBuilder().WithCreated(dateValid).Build();

            AuthenticateResponse expectedResult = new(user, GENERATED_JWT_TOKEN, generatedRefreshToken.Token);

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, ""))
                .Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(It.IsAny<UserDb>())).Returns(user);
            _jwtUtils.Setup(n => n.GenerateJwtToken(It.IsAny<User>())).Returns(GENERATED_JWT_TOKEN);
            _jwtUtils.Setup(n => n.GenerateRefreshToken(IP_ADDRESS)).Returns(generatedRefreshToken);
            _dateTimeProvider.Setup(n => n.Now).Returns(now);


            var result = _sut.Authenticate(request, IP_ADDRESS);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void should_throw_exception_when_cant_get_user_with_given_username()
        {
            AuthenticateRequest request = createFakeRequest();
            List<UserDb> usersFromDb = new();

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, ""))
                .Returns(usersFromDb);

            _sut.Invoking(n=>n.Authenticate(request, IP_ADDRESS)).Should().Throw<Exception>()
                .WithMessage(ApiResponses.UsernameOrPasswordIsIncorrect);
        }

        [Fact]
        public void should_throw_exception_when_wrong_password_passed()
        {
            AuthenticateRequest request = createFakeRequest();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("any other password than in request model");
            UserDb user = new UserBuilder()
                .WithUsername(request.Username).WithPasswordHash(hashedPassword).Build().Map();
            
            List<UserDb> usersFromDb = new() { user };

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, ""))
                .Returns(usersFromDb);

            _sut.Invoking(n => n.Authenticate(request, IP_ADDRESS)).Should().Throw<Exception>()
                .WithMessage(ApiResponses.UsernameOrPasswordIsIncorrect);
        }

        [Fact]
        public void should_add_new_refresh_token_to_users_tokens()
        {
            AuthenticateRequest request = createFakeRequest();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            User user = new UserBuilder()
                .WithUsername(request.Username).WithPasswordHash(hashedPassword).Build();
            List<UserDb> usersFromDb = new() { user.Map() };

            DateTime now = DateTime.UtcNow;
            DateTime dateValid = now.AddDays(_appSettings.Value.RefreshTokenTTL + 5); 
            RefreshToken generatedRefreshToken = new RefreshTokenBuilder().WithCreated(dateValid).Build();

            User result = null;
            Action<User, UserDb> mapUserToUserDb = (usr, usrfromdb) => { result = usr; };
            List<RefreshToken> expectedResult = new() { generatedRefreshToken };


            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, ""))
                .Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(It.IsAny<UserDb>())).Returns(user);
            _jwtUtils.Setup(n => n.GenerateJwtToken(It.IsAny<User>())).Returns(GENERATED_JWT_TOKEN);
            _jwtUtils.Setup(n => n.GenerateRefreshToken(IP_ADDRESS)).Returns(generatedRefreshToken);
            _dateTimeProvider.Setup(n => n.Now).Returns(now);
            _mapper.Setup(n => n.Map(It.IsAny<User>(), It.IsAny<UserDb>())).Callback(mapUserToUserDb);


            _sut.Authenticate(request, IP_ADDRESS);

            result.RefreshTokens.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void should_remove_old_refresh_tokens()
        {
            AuthenticateRequest request = createFakeRequest();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            User user = new UserBuilder()
                .WithUsername(request.Username).WithPasswordHash(hashedPassword).Build();
            List<UserDb> usersFromDb = new() { user.Map() };


            DateTime now = DateTime.UtcNow;
            DateTime dateExpired = now.Subtract(TimeSpan.FromDays(3));
            var inactiveTokens = new RefreshTokenBuilder().Inactive().WithCreated(dateExpired).Build(5);


            user.RefreshTokens = inactiveTokens;

            List<RefreshToken> expectedResult = new();
            User result = null;
            Action<User, UserDb> mapUserToUserDb = (usr, usrfromdb) => { result = usr; };

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, ""))
                .Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(It.IsAny<UserDb>())).Returns(user);
            _jwtUtils.Setup(n => n.GenerateJwtToken(It.IsAny<User>()));
            _jwtUtils.Setup(n => n.GenerateRefreshToken(IP_ADDRESS)).Returns(inactiveTokens[0]);
            _dateTimeProvider.Setup(n => n.Now).Returns(now);
            _mapper.Setup(n => n.Map(It.IsAny<User>(), It.IsAny<UserDb>())).Callback(mapUserToUserDb);

            _sut.Authenticate(request, IP_ADDRESS);

            result.RefreshTokens.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void should_commit_changes()
        {
            AuthenticateRequest request = createFakeRequest();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            User user = new UserBuilder().WithUsername(request.Username).WithPasswordHash(hashedPassword).Build();
            List<UserDb> usersFromDb = new() { user.Map() };

            bool commited = false;
            Action commit = () => { commited = true; };

            _users.Setup(n => n.Get(It.IsAny<Expression<Func<UserDb, bool>>>(), null, ""))
                .Returns(usersFromDb);
            _mapper.Setup(n => n.Map<User>(It.IsAny<UserDb>())).Returns(user);
            _jwtUtils.Setup(n => n.GenerateJwtToken(It.IsAny<User>()));
            _jwtUtils.Setup(n => n.GenerateRefreshToken(IP_ADDRESS)).Returns(new RefreshToken());
            _dateTimeProvider.Setup(n => n.Now).Returns(DateTime.UtcNow);

            _usersUnitOfWork.Setup(n => n.Commit()).Callback(commit);

            _sut.Authenticate(request, IP_ADDRESS);

            commited.Should().BeTrue();
        }

        private AuthenticateRequest createFakeRequest()
        {
            return new Faker<AuthenticateRequest>()
                .RuleFor(n=>n.Password, b=>b.Name.FirstName())
                .RuleFor(n=>n.Username, b=>b.Name.FirstName())
                .Generate();
        }
    }
}
