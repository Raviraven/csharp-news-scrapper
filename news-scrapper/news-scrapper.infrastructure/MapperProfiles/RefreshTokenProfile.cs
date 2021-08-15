using AutoMapper;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models;

namespace news_scrapper.infrastructure.MapperProfiles
{
    public class RefreshTokenProfile : Profile
    {
        public RefreshTokenProfile()
        {
            CreateMap<RefreshTokenDb, RefreshToken>().ReverseMap();
        }
    }
}
