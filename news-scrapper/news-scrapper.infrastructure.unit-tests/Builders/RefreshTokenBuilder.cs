using Bogus;
using news_scrapper.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.infrastructure.unit_tests.Builders
{
    public class RefreshTokenBuilder
    {
        private DateTime _created;

        private bool _shouldGenerateCreated = true;

        private bool _generateInactive = false;

        public RefreshTokenBuilder WithCreated(DateTime created)
        {
            _created = created;
            _shouldGenerateCreated = false;
            return this;
        }

        public RefreshTokenBuilder Inactive()
        {
            _generateInactive = true;
            return this;
        }

        public RefreshToken Build()
        {
            return Build(1).ElementAt(0);
        }

        public List<RefreshToken> Build(int count)
        {
            return new Faker<RefreshToken>()
                .RuleFor(n => n.Id, b => b.Random.Int())
                .RuleFor(n => n.Token, b => b.Name.FirstName())
                .RuleFor(n => n.Expires, b => (_generateInactive) ? b.Date.Past() : b.Date.Future())
                .RuleFor(n => n.Created, b => (_shouldGenerateCreated) ? b.Date.Future() : _created)
                .RuleFor(n => n.CreatedByIp, b => b.Internet.IpAddress().ToString())
                .RuleFor(n=>n.Revoked, b=> (_generateInactive) ? DateTime.UtcNow : null)
                //.RuleFor(n=>n.RevokedByIp, b=>b.Internet.IpAddress().ToString())
                //.RuleFor(n=>n.ReplacedByToken, b=>b.)
                //.RuleFor(n=>n.ReasonRevoked, b=>b.)
                //.RuleFor(n => n.IsActive, b => false)
                //.RuleFor(n => n.IsRevoked, b => false)
                //.RuleFor(n => n.IsExpired, b => false)
                .Generate(count);
        }
    }
}
