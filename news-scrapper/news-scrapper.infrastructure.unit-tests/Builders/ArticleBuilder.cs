using Bogus;
using news_scrapper.domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace news_scrapper.infrastructure.unit_tests.Builders
{
    public class ArticleBuilder
    {
        public Article Build()
        {
            return Build(1).ElementAt(0);
        }

        public List<Article> Build(int count)
        {
            return new Faker<Article>()
                .RuleFor(n=>n.DateScrapped, b=>b.Date.Recent())
                .RuleFor(n=>n.Description, b=>b.Name.FirstName())
                .RuleFor(n=>n.Id, b=>b.Random.Int())
                .RuleFor(n=>n.ImageUrl, b=>b.Internet.Url())
                .RuleFor(n=>n.Title, b=>b.Name.FirstName())
                .RuleFor(n=>n.Url, b=>b.Internet.Url())
                .RuleFor(n=>n.WebsiteDetailsId, b=>b.Random.Int())
                .Generate(count);
        }
    }
}
