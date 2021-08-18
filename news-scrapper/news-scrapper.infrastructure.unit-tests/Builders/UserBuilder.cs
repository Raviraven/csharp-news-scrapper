using Bogus;
using news_scrapper.domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace news_scrapper.infrastructure.unit_tests.Builders
{
    public class UserBuilder
    {
        private string _username;
        private string _passwordHash;
        private List<RefreshToken> _refreshTokens = new();

        private bool _shouldGenerateUsername = true;
        private bool _shouldGeneratePasswordHash = true;

        public UserBuilder WithUsername(string username)
        {
            _username = username;
            _shouldGenerateUsername = false;
            return this;
        }

        public UserBuilder WithPasswordHash(string passwordHash)
        {
            _passwordHash = passwordHash;
            _shouldGeneratePasswordHash = false;
            return this;
        }

        public UserBuilder WithRefreshTokens(List<RefreshToken> refreshTokens)
        {
            _refreshTokens = refreshTokens;
            return this;
        }

        public User Build()
        {
            return Build(1).ElementAt(0);
        }

        public List<User> Build(int count)
        {
            return new Faker<User>()
                .RuleFor(n => n.Id, b => b.Random.Int())
                .RuleFor(n => n.FirstName, b => b.Name.FirstName())
                .RuleFor(n => n.LastName, b => b.Name.LastName())
                .RuleFor(n => n.PasswordHash, b => (_shouldGeneratePasswordHash) ? b.Name.FirstName() : _passwordHash)
                .RuleFor(n => n.Username, b => (_shouldGenerateUsername) ? b.Name.FirstName() : _username)
                .RuleFor(n => n.RefreshTokens, b => _refreshTokens)
                .Generate(count);
        }

    }
}
