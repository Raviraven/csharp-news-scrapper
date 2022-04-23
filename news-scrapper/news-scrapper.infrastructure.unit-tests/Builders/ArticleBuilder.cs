using System;
using Bogus;
using news_scrapper.domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace news_scrapper.infrastructure.unit_tests.Builders
{
    public class ArticleBuilder
    {
        private DateTime _dateScrapped;

        private bool _shouldGenerateDate = true;

        public ArticleBuilder WithDate(DateTime dateScrapped)
        {
            _dateScrapped = dateScrapped;
            _shouldGenerateDate = false;
            return this;
        }

        public Article Build()
        {
            return Build(1).ElementAt(0);
        }

        public List<Article> Build(int count)
        {
            return new Faker<Article>()
                .RuleFor(n=>n.DateScrapped, b=> (_shouldGenerateDate) ? b.Date.Future() : _dateScrapped)
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
