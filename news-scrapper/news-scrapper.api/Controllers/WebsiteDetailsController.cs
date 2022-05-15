using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using news_scrapper.api.Attributes;
using news_scrapper.application.Interfaces;
using news_scrapper.domain.Exceptions;
using news_scrapper.domain.Models;
using news_scrapper.domain.Models.WebsiteDetails;
using news_scrapper.domain.ResponseViewModels;
using System.Collections.Generic;

namespace news_scrapper.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class WebsiteDetailsController : ControllerBase
    {
        private readonly ILogger<WebsiteDetailsController> _logger;
        private IWebsiteDetailsService _websiteDetailsService { get; set; }

        public WebsiteDetailsController(ILogger<WebsiteDetailsController> logger, IWebsiteDetailsService websiteDetailsService)
        {
            _logger = logger;
            _websiteDetailsService = websiteDetailsService;
        }

        [HttpGet]
        public ActionResult<List<WebsiteDetails>> GetAll()
        {
            var userId = (HttpContext.Items["User"] as User).Id;
            var result = _websiteDetailsService.GetAll(userId); //_websiteDetailsService.GetAll();

            if (result.Count == 0)
                throw new KeyNotFoundException();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<List<WebsiteDetails>> Get(int id)
        {
            var result =  _websiteDetailsService.Get(id);

            if (result is null)
                throw new KeyNotFoundException();

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<WebsiteDetailsResponseViewModel> Add(WebsiteDetails website)
        {
            WebsiteDetailsResponseViewModel result = new();
            try
            { 
                var addedWebsite = _websiteDetailsService.Add(website);
                result.WebsiteDetails.Add(addedWebsite);
            }
            catch (InvalidWebsiteDetailsException ex)
            {
                result.ErrorMessages.Add(ex.Message);
            }
            return Ok(result);
        }

        [HttpPut]
        public ActionResult<WebsiteDetailsResponseViewModel> Save(WebsiteDetails website)
        {
            WebsiteDetailsResponseViewModel result = new();
            try
            { 
                var savedWebsite = _websiteDetailsService.Save(website);
                result.WebsiteDetails.Add(savedWebsite);
            }
            catch (InvalidWebsiteDetailsException ex)
            {
                result.ErrorMessages.Add(ex.Message);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(int id)
        {
            var result = _websiteDetailsService.Delete(id);
            return Ok(result);
        }
    }
}
