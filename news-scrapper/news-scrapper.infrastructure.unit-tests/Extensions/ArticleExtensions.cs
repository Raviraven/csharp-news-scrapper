using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models;
using System.Collections.Generic;

namespace news_scrapper.infrastructure.unit_tests
{
    public static class ArticleExtensions
    {
        public static Article Map(this ArticleDb article)
        {
            if (article is null) return null;
            return new() { 
                DateScrapped = article.DateScrapped,
                Description = article.Description,
                Id = article.Id,
                ImageUrl = article.ImageUrl,
                Title = article.Title,
                Url = article.Url,
                WebsiteDetailsId = article.WebsiteDetailsId
            };
        }

        public static List<Article> Map(this List<ArticleDb> articles)
        {
            if (articles is null) return null;

            List<Article> toReturn = new();

            foreach (var article in articles)
            {
                toReturn.Add(
                    new()
                    {
                        DateScrapped = article.DateScrapped,
                        Description = article.Description,
                        Id = article.Id,
                        ImageUrl = article.ImageUrl,
                        Title = article.Title,
                        Url = article.Url,
                        WebsiteDetailsId = article.WebsiteDetailsId
                    }
                );
            }
            return toReturn;
        }

        public static ArticleDb Map(this Article article)
        {
            if (article is null) return null;
            return new()
            {
                DateScrapped = article.DateScrapped,
                Description = article.Description,
                Id = article.Id,
                ImageUrl = article.ImageUrl,
                Title = article.Title,
                Url = article.Url,
                WebsiteDetailsId = article.WebsiteDetailsId
            };
        }

        public static List<ArticleDb> Map(this List<Article> articles)
        {
            if (articles is null) return null;

            List<ArticleDb> toReturn = new();

            foreach (var article in articles)
            {
                toReturn.Add(
                    new()
                    {
                        DateScrapped = article.DateScrapped,
                        Description = article.Description,
                        Id = article.Id,
                        ImageUrl = article.ImageUrl,
                        Title = article.Title,
                        Url = article.Url,
                        WebsiteDetailsId = article.WebsiteDetailsId
                    }
                );
            }
            return toReturn;
        }
    }
}
