using FluentAssertions;
using news_scrapper.domain.Models;
using news_scrapper.infrastructure.unit_tests.Builders;
using news_scrapper.resources;
using System.Collections.Generic;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.UserServiceTests
{
    public class GetByIdTest : BaseTest
    {
        [Fact]
        public void should_return_user()
        {
            int id = 1123;
            var user = new UserBuilder().Build();
            var userFromDb = user.Map();

            _users.Setup(n => n.GetById(id)).Returns(userFromDb);
            _mapper.Setup(n => n.Map<User>(userFromDb)).Returns(user);

            var result = _sut.GetById(id);

            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void should_throw_exception_when_user_not_found()
        {
            int id = 1123;
            User user = null;
            var userFromDb = user.Map();

            _users.Setup(n => n.GetById(id)).Returns(userFromDb);
            _mapper.Setup(n => n.Map<User>(userFromDb)).Returns(user);

            _sut.Invoking(n => n.GetById(id))
                .Should()
                .Throw<KeyNotFoundException>()
                .WithMessage(ApiResponses.UserNotFound);
        }
    }
}
