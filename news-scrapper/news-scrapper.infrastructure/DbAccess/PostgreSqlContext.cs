using Microsoft.EntityFrameworkCore;
using news_scrapper.domain.DBModels;

namespace news_scrapper.infrastructure.DbAccess
{
    public class PostgreSqlContext : DbContext
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
