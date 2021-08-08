using news_scrapper.application.Interfaces;
using news_scrapper.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.infrastructure
{
    public class PagesScrapperService : IPagesScrapperService
    {
        private IHtmlScrapper _htmlScrapper { get; set; }
        private IWebsiteService _websiteService { get; set; }

        public PagesScrapperService(IHtmlScrapper htmlScrapper,
            IWebsiteService websiteService)
        {
            _htmlScrapper = htmlScrapper;
            _websiteService = websiteService;
        }

        public async Task<List<Article>> ScrapAll()
        {
            List<WebsiteDetails> websitesToScrap = mockDbPages();

            List<Article> articles = new();

            foreach (var website in websitesToScrap)
            {
                var rawHtml = await _websiteService.GetRawHtml(website.Url);
                articles.AddRange(await _htmlScrapper.Scrap(website, rawHtml));
            }

            return articles;
        }

        private static List<WebsiteDetails> mockDbPages()
        {
            List<WebsiteDetails> websitesToScrap = new();

            websitesToScrap.Add(new("https://skalawyzwania.pl/", "//*[@id=\"lp-boxes-1\"]", "div", "lp-box box",
               "h4", "lp-box-title", "div", "lp-box-text-inside", "attachment-roseta-lpbox-1 size-roseta-lpbox-1"));
            websitesToScrap.Add(new("https://www.cdaction.pl/", "//*[@id=\"newsy\"]/div", "div", "news not_last_news",
                "h3", "", "td", "td_lead", "news_list_img"));
            websitesToScrap.Add(new("https://lowcygier.pl/", "//*[@id=\"page\"]/div/div[1]/div[2]/main", "article",
                "post-widget post entry clearfix", "h2", "post-title", "div", "text-wrapper lead-wrapper", "img-fluid rounded"));
            return websitesToScrap;
        }
    }
}
