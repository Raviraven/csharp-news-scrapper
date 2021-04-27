using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using news_scrapper.application.Interfaces;
using news_scrapper.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace news_scrapper.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private readonly ILogger<PageController> _logger;
        private IHtmlScrapper _htmlScrapper { get; set; }

        public PageController(ILogger<PageController> logger, IHtmlScrapper htmlScrapper)
        {
            _logger = logger;
            _htmlScrapper = htmlScrapper;
        }


        [HttpGet]
        public async Task<List<Article>> GetScrappedPage()
        {
            var result0 = await _htmlScrapper.Scrap("https://skalawyzwania.pl/", "//*[@id=\"lp-boxes-1\"]", "div", "lp-box box", 
                "h4", "lp-box-title", "div", "lp-box-text-inside", "attachment-roseta-lpbox-1 size-roseta-lpbox-1");

            ////*[@id="lp-boxes-1"]/div

            var result = await _htmlScrapper.Scrap("https://www.cdaction.pl/", "//*[@id=\"newsy\"]/div", "div", "news not_last_news", 
                "h3", "", "td", "td_lead", "news_list_img");

            var result2 = await _htmlScrapper.Scrap("https://lowcygier.pl/", "//*[@id=\"page\"]/div/div[1]/div[2]/main", "article",
                "post-widget post entry clearfix", "h2", "post-title", "div", "text-wrapper lead-wrapper", "img-fluid rounded");

            List <Article> final = new();
            final.AddRange(result0);
            //final.AddRange(result);
            //final.AddRange(result2);

            return final;
        }
    }
}
