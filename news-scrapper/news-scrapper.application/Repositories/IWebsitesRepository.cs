using news_scrapper.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.application.Repositories
{
    public interface IWebsitesRepository
    {
        Task<List<WebsiteDetails>> GetAll();
    }
}
