using Microsoft.AspNetCore.Mvc;
using news_scrapper.api.Attributes;
using news_scrapper.application.Data;
using news_scrapper.domain.Models.Categories;
using System.Collections.Generic;

namespace news_scrapper.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private ICategoriesService _categoriesService { get; set; }

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public ActionResult<List<Category>> GetAll()
        {
            var categories = _categoriesService.Get();

            if (categories is null || categories.Count == 0)
                throw new KeyNotFoundException();

            return categories;
        }

        [HttpGet("{id}")]
        public ActionResult<Category> GetById(int id)
        {
            var category = _categoriesService.Get(id);
            if (category is null)
                throw new KeyNotFoundException("");
            
            return category;
        }

        [HttpPost]
        public ActionResult<CategoryAdd> Add(CategoryAdd category)
        {
            var addedCategory = _categoriesService.Add(category);
            return addedCategory;
        }

        [HttpPut]
        public ActionResult<Category> Save(CategoryEdit category)
        {
            var savedCategory = _categoriesService.Save(category);
            return savedCategory;
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(int id)
        {
            bool deleted = _categoriesService.Delete(id);
            return deleted;
        }
    }
}
