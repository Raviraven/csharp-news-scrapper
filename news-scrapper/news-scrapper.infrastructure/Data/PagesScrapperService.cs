using news_scrapper.application.Interfaces;
using news_scrapper.application.Repositories;
using news_scrapper.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.infrastructure.Data
{
    public class PagesScrapperService : IPagesScrapperService
    {
        private IHtmlScrapper _htmlScrapper { get; set; }
        private IWebsiteService _websiteService { get; set; }
        private IWebsitesRepository _websiteRepository { get; set; }

        public PagesScrapperService(IHtmlScrapper htmlScrapper,
            IWebsiteService websiteService,
            IWebsitesRepository websiteRepository)
        {
            _htmlScrapper = htmlScrapper;
            _websiteService = websiteService;
            _websiteRepository = websiteRepository;
        }

        public async Task<List<Article>> ScrapAll()
        {
            List<WebsiteDetails> websitesToScrap = await _websiteRepository.GetAll();

            List<Article> articles = new();

            foreach (var website in websitesToScrap)
            {
                var rawHtml = await _websiteService.GetRawHtml(website.Url);

                //Regex tagRegex = new Regex(@"<[^>]+>");
                //bool hasTags = tagRegex.IsMatch(myString);

                articles.AddRange(await _htmlScrapper.Scrap(website, rawHtml));
            }

            return articles;
        }
    }
}
