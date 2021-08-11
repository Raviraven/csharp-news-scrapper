using AutoMapper;
using news_scrapper.domain;
using news_scrapper.domain.DBModels;

namespace news_scrapper.infrastructure.MapperProfiles
{
    public class WebsiteDetailsProfile : Profile
    {
        public WebsiteDetailsProfile()
        {
            CreateMap<WebsiteDetails, WebsiteDetailsDb>().ReverseMap();
        }
    }
}
