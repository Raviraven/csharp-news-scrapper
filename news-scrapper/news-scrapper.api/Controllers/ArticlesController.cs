using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using news_scrapper.application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace news_scrapper.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ILogger<PageController> _logger;
        private IArticlesService _articlesService { get; set; }

        public ArticlesController(ILogger<PageController> logger, IArticlesService articlesService)
        {
            _logger = logger;
            _articlesService = articlesService;
        }

        [HttpGet("/scrap")]
        public async Task<ActionResult> Scrap()
        {
            await _articlesService.Scrap();
            return Ok();
        }

    }
}
