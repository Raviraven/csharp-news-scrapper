using Microsoft.EntityFrameworkCore;
using news_scrapper.application.DbAccess;
using news_scrapper.domain.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.infrastructure.DbAccess
{
    public class PostgreSqlContext : DbContext, IPostgreSqlContext
    {
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options)
            :base(options)
        {
        }

        public DbSet<WebsiteDetailsDb> WebsitesDetails { get; set; }

        public void MigrateDatabase()
        {
            this.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
    }
}
