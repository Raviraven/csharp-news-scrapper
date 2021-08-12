using news_scrapper.application.Repositories;
using news_scrapper.application.UnitsOfWork;
using news_scrapper.domain.DBModels;
using news_scrapper.infrastructure.DbAccess;
using news_scrapper.infrastructure.Repositories;

namespace news_scrapper.infrastructure.UnitsOfWork
{
    public class ArticlesUnitOfWork : IArticlesUnitOfWork
    {
        private PostgreSqlContext _context;
        private BaseRepository<ArticleDb> _articles;

        public ArticlesUnitOfWork(PostgreSqlContext context)
        {
            _context = context;
        }

        public IRepository<ArticleDb> Articles 
        { 
            get
            {
                return _articles??= new BaseRepository<ArticleDb>(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
