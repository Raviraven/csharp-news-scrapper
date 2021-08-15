using AutoMapper;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models;

namespace news_scrapper.infrastructure.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDb, User>().ReverseMap();
        }
    }
}
