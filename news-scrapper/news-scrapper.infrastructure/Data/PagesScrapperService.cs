using AutoMapper;
using news_scrapper.application.Data;
using news_scrapper.application.Interfaces;
using news_scrapper.application.Repositories;
using news_scrapper.domain;
using news_scrapper.domain.ResponseViewModels;
using news_scrapper.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace news_scrapper.infrastructure.Data
{
    public class PagesScrapperService : IPagesScrapperService
    {
        private IHtmlScrapper _htmlScrapper { get; set; }
        private IWebsiteService _websiteService { get; set; }
        private IWebsitesRepository _websiteRepository { get; set; }
        private IMapper _mapper { get; set; }


        public PagesScrapperService(IHtmlScrapper htmlScrapper,
            IWebsiteService websiteService,
            IWebsitesRepository websiteRepository, IMapper mapper)
        {
            _htmlScrapper = htmlScrapper;
            _websiteService = websiteService;
            _websiteRepository = websiteRepository;
            _mapper = mapper;
        }

        public async Task<ArticlesResponseViewModel> ScrapAll()
        {
            List<WebsiteDetails> websitesToScrap = _mapper.Map<List<WebsiteDetails>>(_websiteRepository.GetAll());

            if (noWebsitesInDb(websitesToScrap))
            {
                return new ArticlesResponseViewModel 
                { 
                    ErrorMessages = new List<string> 
                    { 
                        ApiResponses.ThereAreNoWebsitesToScrap 
                    }
                };
            }

            return await scrapWebsites(websitesToScrap);
        }

        private async Task<ArticlesResponseViewModel> scrapWebsites(List<WebsiteDetails> websitesToScrap)
        {
            ArticlesResponseViewModel result = new();
            
            foreach (var website in websitesToScrap)
            {
                var rawHtml = await _websiteService.GetRawHtml(website.Url);

                var scrappedWebsiteResult = scrapSingleWebsite(website, rawHtml);
                
                result.Articles.AddRange(scrappedWebsiteResult.Articles);
                result.ErrorMessages.AddRange(scrappedWebsiteResult.ErrorMessages);
            }
            
            return result;
        }

        private ArticlesResponseViewModel scrapSingleWebsite(WebsiteDetails website, string rawHtml)
        {
            ArticlesResponseViewModel result = new();
            
            if (isStringHtml(rawHtml))
            {
                (List<Article> articles, List<string> errors) = _htmlScrapper.Scrap(website, rawHtml);

                result.Articles.AddRange(articles);
                result.ErrorMessages.AddRange(errors);

                return result;
            }
            
            result.ErrorMessages.Add(rawHtml);
            return result;    
        }

        private bool noWebsitesInDb(List<WebsiteDetails> websitesToScrap)
        {
            return websitesToScrap is null || websitesToScrap.Count == 0;
        }

        private bool isStringHtml(string rawHtml)
        {
            Regex tagRegex = new(@"<[^>]+>");
            return tagRegex.IsMatch(rawHtml);
        }
    }
}
