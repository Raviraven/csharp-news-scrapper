using AutoMapper;
using news_scrapper.application.Data;
using news_scrapper.application.UnitsOfWork;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task Scrap()
        {
            var scrappedArticles = await _pagesScrapperService.ScrapAll();
            //what about error messages? just pass it  ? 

            List<Article> articlesToAdd = new();

            foreach (var article in scrappedArticles.Articles)
            {
                var dbArticle = _articlesUnitOfWork.Articles.Get(
                    filter: n => n.WebsiteDetailsId == article.WebsiteDetailsId && n.Title == article.Title)
                .FirstOrDefault();

                if (dbArticle is null)
                    articlesToAdd.Add(article);
            }

            _articlesUnitOfWork.Articles.InsertRange(_mapper.Map<List<ArticleDb>>(articlesToAdd));
            _articlesUnitOfWork.Commit();


            //returns some kind of results...
            var addedArticles = articlesToAdd.Count();
        }
    }
}
