using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models.Categories;

namespace news_scrapper.infrastructure.unit_tests.Extensions
{
    public static class CategoryAddExtensions
    {
        public static CategoryAdd Map(this CategoryDb category)
        {
            return new CategoryAdd
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static CategoryDb Map(this CategoryAdd category)
        {
            return new CategoryDb
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
