using news_scrapper.domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace news_scrapper.application.Interfaces
{
    public interface IWebsiteDetailsService
    {
        WebsiteDetails Add(WebsiteDetails websiteDetails);
        bool Delete(int id);
        Task<List<WebsiteDetails>> GetAll();
        WebsiteDetails Get(int id);
        WebsiteDetails Save(WebsiteDetails websiteDetails);
    }
}
