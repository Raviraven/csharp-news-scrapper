using AutoMapper;
using news_scrapper.application.Interfaces;
using news_scrapper.application.Repositories;
using news_scrapper.domain;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Exceptions;
using news_scrapper.resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace news_scrapper.infrastructure.Data
{
    public class WebsiteDetailsService : IWebsiteDetailsService
    {
        private IWebsitesRepository _websitesRepository { get; set; }
        private IMapper _mapper { get; set; }

        public WebsiteDetailsService(IWebsitesRepository websitesRepository, IMapper mapper)
        {
            _websitesRepository = websitesRepository;
            _mapper = mapper;
        }

        public WebsiteDetails Add(WebsiteDetails websiteDetails)
        {
            validateWebsiteDetails(websiteDetails);

            var websiteDetailsDb = _mapper.Map<WebsiteDetailsDb>(websiteDetails);
            var result = _mapper.Map<WebsiteDetails>(_websitesRepository.Add(websiteDetailsDb));

            _websitesRepository.Commit();

            return result;
        }

        public bool Delete(int id)
        {
            _websitesRepository.Delete(id);
            _websitesRepository.Commit();
            return true;
        }

        public List<WebsiteDetails> GetAll()
        {
            var result = _mapper.Map<List<WebsiteDetails>>(_websitesRepository.GetAll());
            return result;
        }

        public WebsiteDetails Get(int id)
        {
            var result = _mapper.Map<WebsiteDetails>(_websitesRepository.Get(id));
            return result;
        }

        public WebsiteDetails Save(WebsiteDetails websiteDetails)
        {
            validateWebsiteDetails(websiteDetails);

            var websiteDetailsDb = _mapper.Map<WebsiteDetailsDb>(websiteDetails);
            var result = _mapper.Map<WebsiteDetails>(_websitesRepository.Save(websiteDetailsDb));

            _websitesRepository.Commit();

            return result;
        }


        private void validateWebsiteDetails(WebsiteDetails websiteDetails)
        {
            if (websiteDetails is null)
                throw new InvalidWebsiteDetailsException(ApiResponses.WebsiteDetailsCannotBeNull);

            if (string.IsNullOrEmpty(websiteDetails.Url))
                throw new InvalidWebsiteDetailsException(ApiResponses.WebsiteDetailsUrlCannotBeNullOrEmpty);

            if (string.IsNullOrEmpty(websiteDetails.MainNodeXPathToNewsContainer))
                throw new InvalidWebsiteDetailsException(ApiResponses.WebsiteDetailsXpathCannotBeNullOrEmpty);

            if (string.IsNullOrEmpty(websiteDetails.NewsNodeTag))
                throw new InvalidWebsiteDetailsException(ApiResponses.WebsiteDetailsNewsNodeCannotBeNullOrEmpty);
        }
    }
}
