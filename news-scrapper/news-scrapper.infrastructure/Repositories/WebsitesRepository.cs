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
        List<WebsiteDetails> websites = new();

        public WebsiteDetails Add(WebsiteDetails websiteDetails)
        {
            int id = generateId();
            websiteDetails.Id = id;
            websites.Add(websiteDetails);

            return websiteDetails;
        }

        private int generateId()
        {
            if (websites is null || websites.Count == 0)
                return 1;

            return websites.OrderByDescending(n => n.Id).First().Id++;
        }

        public bool Delete(int id)
        {
            var toRemove = websites.FirstOrDefault(n => n.Id == id);

            if (toRemove is null)
                throw new Exception($"Can't find website detail with id: '{id}', so it can't be deleted.");

            return websites.Remove(toRemove);
        }

        public WebsiteDetails Get(int id)
        {
            return websites.FirstOrDefault(n => n.Id == id);
        }

        public async Task<List<WebsiteDetails>> GetAll()
        {
            //websites.Add(new() { Url = "https://test.test" });

            //websites.Add(new("https://skalawyzwania.pl/", "//*[@id=\"lp-boxes-1\"]", "div", "lp-box box",
            //   "h4", "lp-box-title", "div", "lp-box-text-inside", "attachment-roseta-lpbox-1 size-roseta-lpbox-1"));
            //websites.Add(new("https://www.cdaction.pl/", "//*[@id=\"newsy\"]/div", "div", "news not_last_news",
            //    "h3", "", "td", "td_lead", "news_list_img"));
            //websites.Add(new("https://lowcygier.pl/", "//*[@id=\"page\"]/div/div[1]/div[2]/main", "article",
            //    "post-widget post entry clearfix", "h2", "post-title", "div", "text-wrapper lead-wrapper", "img-fluid rounded"));

            return await Task.FromResult(websites);
        }

        public WebsiteDetails Save(WebsiteDetails websiteDetails)
        {
            var websiteToEdit = websites.FirstOrDefault(n => n.Id == websiteDetails.Id);

            if (websiteToEdit is null)
                throw new Exception($"Website detail with id: '{websiteDetails.Id}' not found.");

            websiteToEdit.UpdateValues(websiteDetails);
            return websiteToEdit;
        }
    }
}
