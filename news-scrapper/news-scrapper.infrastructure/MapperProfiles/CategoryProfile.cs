using AutoMapper;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models.Categories;

namespace news_scrapper.infrastructure.MapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDb, Category>();
            CreateMap<CategoryAdd, Category>();
            CreateMap<CategoryAdd, CategoryDb>().ReverseMap();
            CreateMap<CategoryEdit, Category>().ReverseMap();
            CreateMap<CategoryEdit, CategoryDb>().ReverseMap();

            CreateMap<CategoryDb, CategoryWebsiteDetails>().ReverseMap();
        }
    }
}
