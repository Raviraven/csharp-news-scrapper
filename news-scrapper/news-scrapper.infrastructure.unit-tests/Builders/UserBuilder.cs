using Bogus;
using news_scrapper.domain.Models;

namespace news_scrapper.infrastructure.unit_tests.Builders
{
    public class UserBuilder
    {
        public User Build()
        {
            return new Faker<User>()
                .RuleFor(n=>n.Id, b=>b.Random.Int())
                .RuleFor(n=>n.FirstName, b=>b.Name.FirstName())
                .RuleFor(n=>n.LastName, b=>b.Name.LastName())
                .RuleFor(n=>n.PasswordHash, b=>b.Name.FirstName())
                .RuleFor(n=>n.Username, b=>b.Name.FirstName())
                .Generate();
        }
    }
}
