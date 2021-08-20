using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using news_scrapper.api.Attributes;
using news_scrapper.application.Data;
using news_scrapper.domain.Models;
using news_scrapper.resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace news_scrapper.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ArticlesController : ControllerBase
    {
        private readonly ILogger<ArticlesController> _logger;
        private IArticlesService _articlesService { get; set; }

        public ArticlesController(ILogger<ArticlesController> logger, IArticlesService articlesService)
        {
            _logger = logger;
            _articlesService = articlesService;
        }

        [HttpGet("scrap")]
        public async Task<ActionResult<List<string>>> Scrap()
        {
            var result = await _articlesService.Scrap();
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<List<Article>> Get([FromQuery] int page, [FromQuery] int count)
        {
            List<Article> result;

            if (page == 0 && count == 0)
            {
                result = _articlesService.Get();
            }
            else
            {
                result = _articlesService.Get(count, page);
            }

            if (result == null)
                return NotFound(ApiResponses.ArticlesNotFound);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<Article> GetById(int id)
        {
            var result = _articlesService.GetById(id);

            if (result == null)
                return NotFound(string.Format(ApiResponses.ArticleWithIdNotFound, id));

            return Ok(result);
        }

        [HttpGet("new")]
        public ActionResult<List<Article>> GetNew()
        {
            var result = _articlesService.GetNew();

            if (result is null)
                return NotFound(ApiResponses.ArticlesNotFound);

            return Ok(result);
        }
    }
}
