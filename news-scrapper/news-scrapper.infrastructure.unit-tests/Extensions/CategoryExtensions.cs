using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models.Categories;
using System.Collections.Generic;

namespace news_scrapper.infrastructure.unit_tests.Extensions
{
    public static class CategoryExtensions
    {
        public static Category MapToCategory(this CategoryDb category)
        {
            if(category is null) return null;

            return new Category { 
                Id = category.Id,
                Name = category.Name,
                //Websites = category.Websites.ToList(),
            };
        }

        public static List<Category> MapToCategory(this List<CategoryDb> categories)
        {
            var list = new List<Category>();
            foreach (var category in categories)
            {
                list.Add(MapToCategory(category));
            }
            return list;
        }

        public static CategoryDb Map(this Category category)
        {
            if (category is null) return null;

            return new CategoryDb
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static List<CategoryDb> Map(this List<Category> categories)
        {
            var list = new List<CategoryDb>();
            foreach (var category in categories)
            {
                list.Add(Map(category));
            }
            return list;
        }
    }
}
