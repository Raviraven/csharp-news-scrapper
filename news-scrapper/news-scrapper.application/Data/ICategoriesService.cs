using news_scrapper.domain.Models.Categories;
using System.Collections.Generic;

namespace news_scrapper.application.Data
{
    public interface ICategoriesService
    {
        List<Category> Get();
        Category Get(int id);
        Category Get(string name);
        CategoryAdd Add (CategoryAdd category);
        Category Save(CategoryEdit category);
        bool Delete(int id);
    }
}
