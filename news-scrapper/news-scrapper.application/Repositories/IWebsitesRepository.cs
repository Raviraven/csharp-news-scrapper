using news_scrapper.domain;
using news_scrapper.domain.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.application.Repositories
{
    public interface IWebsitesRepository
    {
        WebsiteDetailsDb Add(WebsiteDetailsDb websiteDetails);
        bool Delete(int id);
        List<WebsiteDetailsDb> GetAll();
        WebsiteDetailsDb Get(int id);
        WebsiteDetailsDb Save(WebsiteDetailsDb websiteDetails);

        void Commit();
    }
}
