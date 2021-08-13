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


        public List<Article> Get()
        {
            var orderedArticles = _articlesUnitOfWork.Articles.Get(
                    orderBy: n => n.OrderByDescending(n => n.DateScrapped)
                );

            if (orderedArticles is null || orderedArticles.Count() == 0)
                return null;

            var articlesFromDb = orderedArticles.ToList();
            return _mapper.Map<List<Article>>(articlesFromDb);
        }

        public List<Article> Get(int articlesPerPage, int pageNo)
        {
            var orderedArticles = _articlesUnitOfWork.Articles.Get(
                orderBy: n => n.OrderByDescending(n => n.DateScrapped)
                );

            if (orderedArticles is null || orderedArticles.Count() == 0)
                return null;

            int allPages = getAllAmountOfPages(articlesPerPage, orderedArticles);

            if (pageNo < 0 || pageNo > allPages)
                return null;

            var articlesToReturn = orderedArticles
                .Skip((pageNo - 1) * articlesPerPage)
                .Take(articlesPerPage)
                .ToList();

            return _mapper.Map<List<Article>>(articlesToReturn);
        }

        private static int getAllAmountOfPages(int articlesPerPage, IEnumerable<ArticleDb> orderedArticles)
        {
            double amountOfArticlesPerPage = orderedArticles.Count()
                / (double)articlesPerPage;

            int amountOfArticlesAsInt = Convert.ToInt32(amountOfArticlesPerPage);

            int allPages = (amountOfArticlesAsInt == amountOfArticlesPerPage)
                ? amountOfArticlesAsInt : amountOfArticlesAsInt + 1;
            return allPages;
        }

        public Article GetById(int id)
        {
            var article = _articlesUnitOfWork.Articles.GetById(id);

            if (article is null)
                return null;

            return _mapper.Map<Article>(article);
        }

        public List<Article> GetNew()
        {
            var orderedArticles = _articlesUnitOfWork.Articles.Get(
                    orderBy: n=>n.OrderByDescending(n=>n.DateScrapped)
                );

            if (orderedArticles is null || orderedArticles.Count() == 0)
                return null;

            var newestDate = orderedArticles.First().DateScrapped;
            var newestArticles = orderedArticles
                .Where(n => n.DateScrapped.ToShortDateString() == newestDate.ToShortDateString()
                    && n.DateScrapped.ToShortTimeString() == newestDate.ToShortTimeString())
                .ToList();

            return _mapper.Map<List<Article>>(newestArticles);
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
