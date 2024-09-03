using news_scrapper.domain.DBModels;
using System.Collections.Generic;

namespace news_scrapper.application.Repositories
{
    public interface IWebsitesRepository
    {
        WebsiteDetailsDb Add(WebsiteDetailsDb websiteDetails);
        bool Delete(int id);
        List<WebsiteDetailsDb> GetAll();
        List<WebsiteDetailsDb> GetAll(int userId);
        WebsiteDetailsDb Get(int id);
        WebsiteDetailsDb GetWithCategories(int id);
        WebsiteDetailsDb Save(WebsiteDetailsDb websiteDetails);

        void Commit();
    }
}
