using AutoMapper;
using news_scrapper.application.Data;
using news_scrapper.application.UnitsOfWork;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models.Categories;
using news_scrapper.resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace news_scrapper.infrastructure.Data
{
    public class CategoriesService : ICategoriesService
    {
        public CategoriesService(IMapper mapper, ICategoriesUnitOfWork categoriesUnitOfWork)
        {
            _mapper = mapper;
            _categoriesUnitOfWork = categoriesUnitOfWork;
        }

        private IMapper _mapper { get; set; }
        private ICategoriesUnitOfWork _categoriesUnitOfWork { get; set; }

        public CategoryAdd Add(CategoryAdd category)
        {
            if (category is null)
                throw new ArgumentNullException(ApiResponses.CannotAddNullCategory, innerException: null);

            if (string.IsNullOrEmpty(category.Name))
                throw new ArgumentNullException(ApiResponses.CannotAddCategoryWithNameNullOrEmpty, innerException: null);

            var mappedCategory = _mapper.Map<CategoryDb>(category);

            var websiteDetailsList = _categoriesUnitOfWork.WebsiteDetails
                .Get(n => category.WebsitesIds.Contains(n.id)).ToList();

            mappedCategory.Websites = websiteDetailsList;
            _categoriesUnitOfWork.Categories.Insert(mappedCategory);
            _categoriesUnitOfWork.Commit();

            _mapper.Map(mappedCategory, category);
            //will that store category id?
            return category;
        }

        public bool Delete(int id)
        {
            var entityToDelete = _categoriesUnitOfWork.Categories.GetById(id);

            if (entityToDelete is null)
                throw new ArgumentNullException(string.Format(ApiResponses.CategoryWithGivenIdNotFound, id), innerException: null);

            _categoriesUnitOfWork.Categories.Delete(entityToDelete);
            _categoriesUnitOfWork.Commit();

            return true;
        }

        public List<Category> Get()
        {
            var categories = _categoriesUnitOfWork.Categories.Get(includeProperties: "Websites");
            var result = _mapper.Map<List<Category>>(categories);
            return result;
        }

        public Category Get(int id)
        {
            var category = _categoriesUnitOfWork.Categories.GetById(id);
            var mappedCategory = _mapper.Map<Category>(category);
            return mappedCategory;
        }

        public Category Get(string name)
        {
            var category = _categoriesUnitOfWork.Categories.Get(n => n.Name == name).FirstOrDefault();
            var mappedCategory = _mapper.Map<Category>(category);
            return mappedCategory;
        }

        public Category Save(CategoryEdit category)
        {
            if (category is null)
                throw new ArgumentNullException(ApiResponses.CategoryYouWishToUpdateCannotBeNull, innerException: null);

            var categoryDb = _mapper.Map<CategoryDb>(category);
            var resultDb =_categoriesUnitOfWork.Categories.Update(categoryDb);
            _categoriesUnitOfWork.Commit();

            return _mapper.Map<Category>(resultDb);
        }
    }
}
