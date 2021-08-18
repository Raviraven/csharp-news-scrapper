using FluentAssertions;
using news_scrapper.domain.Models;
using news_scrapper.infrastructure.unit_tests.Builders;
using System.Collections.Generic;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.Tests.UserServiceTests
{
    public class GetAllTest : BaseTest
    {
        [Fact]
        public void should_return_users()
        {
            var users = new UserBuilder().Build(10);
            var usersFromDb = users.Map();

            _users.Setup(n => n.Get(null, null, "")).Returns(usersFromDb);
            _mapper.Setup(n => n.Map<List<User>>(usersFromDb)).Returns(users);

            var result = _sut.GetAll();

            result.Should().BeEquivalentTo(users);
        }
    }
}
