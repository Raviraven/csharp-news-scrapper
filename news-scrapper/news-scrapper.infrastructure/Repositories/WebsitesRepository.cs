using Microsoft.EntityFrameworkCore;
using news_scrapper.application.Repositories;
using news_scrapper.domain.DBModels;
using news_scrapper.infrastructure.DbAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace news_scrapper.infrastructure.Repositories
{
    public class WebsitesRepository : IWebsitesRepository
    {
        private PostgreSqlContext sqlContext { get; set; }

        public WebsitesRepository(PostgreSqlContext sqlContext)
        {
            this.sqlContext = sqlContext;
        }

        public WebsiteDetailsDb Add(WebsiteDetailsDb websiteDetails)
        {
            var test = sqlContext.WebsitesDetails.Add(websiteDetails);
            return websiteDetails;
        }

        public bool Delete(int id)
        {
            var toRemove = sqlContext.WebsitesDetails.FirstOrDefault(n => n.id == id);

            if (toRemove is null)
                throw new Exception($"Can't find website detail with id: '{id}', so it can't be deleted.");

            sqlContext.WebsitesDetails.Remove(toRemove);

            return true;
        }

        public WebsiteDetailsDb Get(int id)
        {
            return sqlContext.WebsitesDetails.FirstOrDefault(n => n.id == id);
        }

        public WebsiteDetailsDb GetWithCategories(int id)
        {
            return sqlContext.WebsitesDetails
                .Include(n=>n.Categories)
                .FirstOrDefault(n => n.id == id);
        }

        public List<WebsiteDetailsDb> GetAll()
        {
            return sqlContext.WebsitesDetails
                .Include(n=>n.Categories)
                .ToList();
        }

        public WebsiteDetailsDb Save(WebsiteDetailsDb websiteDetails)
        {
            sqlContext.WebsitesDetails.Update(websiteDetails);
            return websiteDetails;
        }

        public void Commit()
        {
            sqlContext.SaveChanges();
        }
    }
}
