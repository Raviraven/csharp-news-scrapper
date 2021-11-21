using news_scrapper.application.Repositories;
using news_scrapper.application.UnitsOfWork;
using news_scrapper.domain.DBModels;
using news_scrapper.infrastructure.DbAccess;
using news_scrapper.infrastructure.Repositories;

namespace news_scrapper.infrastructure.UnitsOfWork
{
    public class CategoriesUnitOfWork : ICategoriesUnitOfWork
    {
        private PostgreSqlContext _context;
        private BaseRepository<CategoryDb> _articles;

        public CategoriesUnitOfWork(PostgreSqlContext context)
        {
            _context = context;
        }

        public IRepository<CategoryDb> Categories
        {
            get { return _articles ??= new BaseRepository<CategoryDb>(_context); }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
