using news_scrapper.application.Repositories;
using news_scrapper.domain.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.application.UnitsOfWork
{
    public interface IArticlesUnitOfWork
    {
        IRepository<ArticleDb> Articles { get; }
        void Commit();
    }
}
