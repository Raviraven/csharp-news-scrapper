using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.application.Interfaces
{
    public interface IWebsiteService
    {
        Task<string> GetRawHtml(string url);
    }
}
