using news_scrapper.application.Repositories;
using news_scrapper.application.UnitsOfWork;
using news_scrapper.domain.DBModels;
using news_scrapper.infrastructure.DbAccess;
using news_scrapper.infrastructure.Repositories;
using System;

namespace news_scrapper.infrastructure.UnitsOfWork
{
    public class UsersUnitOfWork : IUsersUnitOfWork, IDisposable
    {
        private PostgreSqlContext _context;
        private BaseRepository<UserDb> _users;

        public UsersUnitOfWork(PostgreSqlContext context)
        {
            _context = context;
        }

        public IRepository<UserDb> Users
        {
            get
            {
                if(_users is null)
                    _users = new BaseRepository<UserDb>(_context);
                return _users;
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
