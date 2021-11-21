using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models.Categories;

namespace news_scrapper.infrastructure.unit_tests.Extensions
{
    public static class CategoryEditExteinsions
    {
        public static Category MapToCategory(this CategoryEdit category)
        {
            if(category is null) return null;

            return new()
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static CategoryDb MapToCategoryDb(this CategoryEdit category)
        {
            if (category is null) return null;

            return new()
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static CategoryEdit MapToCategoryEdit(this Category category)
        {
            if (category is null) return null;

            return new()
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
