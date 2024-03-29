﻿using AutoMapper;
using Moq;
using news_scrapper.application.Repositories;
using news_scrapper.domain;
using news_scrapper.domain.DBModels;
using news_scrapper.infrastructure.Data;

namespace news_scrapper.infrastructure.unit_tests.Tests.WebsiteDetailsServiceTests
{
    public class BaseTest
    {
        protected Mock<IWebsitesRepository> _websitesRepository { get; set; }
        protected Mock<IRepository<CategoryDb>> _categoriesRepository { get; set; }
        protected Mock<IMapper> _mapper { get; set; }

        protected WebsiteDetailsService _sut { get; set; }

        public BaseTest()
        {
            _websitesRepository = new();
            _categoriesRepository = new();

            _mapper = new();

            _sut = new WebsiteDetailsService(_websitesRepository.Object, 
                _mapper.Object, 
                _categoriesRepository.Object);
        }
    }
}
