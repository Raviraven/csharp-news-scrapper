using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace news_scrapper.application.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Delete(TEntity entityToDelete);
        void Delete(object id);
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");


        TEntity GetById(object id);
        IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters);
        TEntity Insert(TEntity entity);
        IEnumerable<TEntity> InsertRange(List<TEntity> entities);
        TEntity Update(TEntity entityToUpdate);

    }
}
