using news_scrapper.application.Interfaces;
using news_scrapper.resources;
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
            try
            {
                return await _httpclient.GetStringAsync(url);
            }
            catch (HttpRequestException)
            {
                return string.Format(ApiResponses.CannotReachSiteWithGivenUrl, url);
            }
            catch (Exception ex)
            {
                return string.Format(ApiResponses.UnexpectedErrorOccured, ex.Message);
            }
        }
    }
}
