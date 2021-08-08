using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using news_scrapper.application.Interfaces;
using news_scrapper.domain;
using news_scrapper.domain.ResponseViewModels;
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
        private IPagesScrapperService _pagesScrapperService{ get; set; }

        public PageController(ILogger<PageController> logger, IPagesScrapperService pagesScrapperService)
        {
            _logger = logger;
            _pagesScrapperService = pagesScrapperService;
        }


        [HttpGet]
        public async Task<ArticlesResponseViewModel> GetScrappedPage()
        {
            return await _pagesScrapperService.ScrapAll();
        }
    }
}
