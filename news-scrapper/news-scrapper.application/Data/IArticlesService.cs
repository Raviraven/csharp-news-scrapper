using news_scrapper.domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace news_scrapper.application.Data
{
    public interface IArticlesService
    {
        List<Article> Get();
        List<Article> Get(int articlesPerPage, int pageNo);
        Article GetById(int id);
        List<Article> GetNew();
        Task<List<string>> Scrap();
    }
}
