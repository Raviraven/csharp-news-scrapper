using news_scrapper.application.Repositories;
using news_scrapper.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.infrastructure.Repositories
{
    public class WebsitesRepository : IWebsitesRepository
    {
        public async Task<List<WebsiteDetails>> GetAll()
        {
            List<WebsiteDetails> websitesToScrap = new();

            websitesToScrap.Add(new("https://skalawyzwania.pl/", "//*[@id=\"lp-boxes-1\"]", "div", "lp-box box",
               "h4", "lp-box-title", "div", "lp-box-text-inside", "attachment-roseta-lpbox-1 size-roseta-lpbox-1"));
            websitesToScrap.Add(new("https://www.cdaction.pl/", "//*[@id=\"newsy\"]/div", "div", "news not_last_news",
                "h3", "", "td", "td_lead", "news_list_img"));
            websitesToScrap.Add(new("https://lowcygier.pl/", "//*[@id=\"page\"]/div/div[1]/div[2]/main", "article",
                "post-widget post entry clearfix", "h2", "post-title", "div", "text-wrapper lead-wrapper", "img-fluid rounded"));

            return await Task.FromResult(websitesToScrap);
        }
    }
}
