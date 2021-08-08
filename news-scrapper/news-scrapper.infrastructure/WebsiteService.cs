using news_scrapper.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.infrastructure
{
    public class WebsiteService : IWebsiteService
    {
        private HttpClient _httpclient { get; set; }

        public WebsiteService(HttpClient httpclient)
        {
            _httpclient = httpclient;
        }

        public async Task<string> GetRawHtml(string url)
        {
            var response = await _httpclient.GetStringAsync(url);
            return response;
        }
    }
}
