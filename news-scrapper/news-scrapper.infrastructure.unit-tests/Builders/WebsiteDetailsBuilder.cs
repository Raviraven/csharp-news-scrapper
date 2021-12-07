using Bogus;
using news_scrapper.domain.Models.WebsiteDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.infrastructure.unit_tests.Builders
{
    public class WebsiteDetailsBuilder
    {
        private string _mainNodeId = "";
        private string _url = "";
        private int _id = 0;

        private bool _shouldGenerateUrl = true;
        private bool _shouldGenerateMainNodeId = true;
        private bool _shouldGenerateId = true;

        public WebsiteDetailsBuilder WithId(int id)
        {
            _id = id;
            _shouldGenerateId = false;
            return this;
        }

        public WebsiteDetailsBuilder WithMainNodeId(string mainNodeId)
        {
            _mainNodeId = mainNodeId;
            _shouldGenerateMainNodeId = false;
            return this;
        }

        public WebsiteDetailsBuilder WithUrl(string url)
        {
            _url = url;
            _shouldGenerateUrl = false;
            return this;
        }

        public WebsiteDetails Build()
        {
            return Build(1).ElementAt(0);
        }

        public List<WebsiteDetails> Build(int count)
        {
            return new Faker<WebsiteDetails>()
                .RuleFor(n=>n.Id, b=> (_shouldGenerateId) ? b.Random.Int() : _id)
                .RuleFor(n => n.MainNodeXPathToNewsContainer, b => (_shouldGenerateMainNodeId) ? 
                        $"//*[@id=\"{b.Name.FirstName()}-id\"]" : $"//*[@id=\"{_mainNodeId}\"]")
                .RuleFor(n => n.NewsNodeTag, b => b.Name.FirstName() + "-news-tag")
                .RuleFor(n => n.NewsNodeClass, b => b.Name.FirstName() + "-news-class")
                .RuleFor(n => n.TitleNodeTag, b => b.Name.FirstName() + "-title-tag")
                .RuleFor(n => n.TitleNodeClass, b => b.Name.FirstName() + "-title-class")
                .RuleFor(n => n.DescriptionNodeTag, b => b.Name.FirstName() + "-description-tag")
                .RuleFor(n => n.DescriptionNodeClass, b => b.Name.FirstName() + "-description-class")
                .RuleFor(n => n.ImgNodeClass, b => b.Name.FirstName() + "-img-class")
                .RuleFor(n => n.Url, b => (_shouldGenerateUrl) ? b.Internet.Url() : _url)
                .Generate(count);
        }

    }
}
