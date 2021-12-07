using news_scrapper.domain.Models.WebsiteDetails;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace news_scrapper.application.Interfaces
{
    public interface IWebsiteDetailsService
    {
        WebsiteDetails Add(WebsiteDetails websiteDetails);
        bool Delete(int id);
        List<WebsiteDetails> GetAll();
        WebsiteDetails Get(int id);
        WebsiteDetails Save(WebsiteDetails websiteDetails);
    }
}
