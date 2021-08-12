using AutoMapper;
using news_scrapper.application.Data;
using news_scrapper.application.UnitsOfWork;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models;
using news_scrapper.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace news_scrapper.infrastructure.Data
{
    public class ArticlesService : IArticlesService
    {
        private IPagesScrapperService _pagesScrapperService { get; set; }
        private IArticlesUnitOfWork _articlesUnitOfWork { get; set; }
        private IMapper _mapper { get; set; }

        public ArticlesService(IPagesScrapperService pagesScrapperService, IArticlesUnitOfWork articlesUnitOfWork, IMapper mapper)
        {
            _pagesScrapperService = pagesScrapperService;
            _articlesUnitOfWork = articlesUnitOfWork;
            _mapper = mapper;
        }

        public async Task<List<string>> Scrap()
        {
            List<string> result = new();
            List<Article> articlesToAdd = new();

            var scrappedArticles = await _pagesScrapperService.ScrapAll();
            result.AddRange(scrappedArticles.ErrorMessages);

            try
            {
               articlesToAdd.AddRange(insertScrappedArticles(scrappedArticles.Articles));
            }
            catch (Exception e)
            {
                result.Add(e.Message);
            }
            
            string summaryMessage = string.Format(ApiResponses.ArticlesAddedAfterScrapping, articlesToAdd.Count());
            result.Add(summaryMessage);

            return result;
        }

        private List<Article> insertScrappedArticles(List<Article> scrappedArticles)
        {
            List<Article> articlesToAdd = new();
            if (scrappedArticles is not null)
            {
                foreach (var article in scrappedArticles)
                {
                    var dbArticle = _articlesUnitOfWork.Articles.Get()
                        .FirstOrDefault(n => n.WebsiteDetailsId == article.WebsiteDetailsId
                        && n.Title == article.Title);

                    //var searchedArticles = _articlesUnitOfWork.Articles.Get(
                    //    filter: n => n.WebsiteDetailsId == article.WebsiteDetailsId && n.Title == article.Title).ToList();
                    //var dbArticle = searchedArticles.FirstOrDefault();

                    if (dbArticle is null)
                        articlesToAdd.Add(article);
                }
            }

            _articlesUnitOfWork.Articles.InsertRange(_mapper.Map<List<ArticleDb>>(articlesToAdd));
            _articlesUnitOfWork.Commit();

            return articlesToAdd;
        }
    }
}
