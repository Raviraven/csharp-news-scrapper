﻿using AutoMapper;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models.WebsiteDetails;

namespace news_scrapper.infrastructure.MapperProfiles
{
    public class WebsiteDetailsProfile : Profile
    {
        public WebsiteDetailsProfile()
        {
            CreateMap<WebsiteDetails, WebsiteDetailsDb>().ReverseMap();
            CreateMap<WebsiteDetailsDb, WebsiteDetailsCategory>();
        }
    }
}
