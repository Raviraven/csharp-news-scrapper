using news_scrapper.application.Interfaces;
using news_scrapper.application.Repositories;
using news_scrapper.domain;
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

        public WebsiteDetailsService(IWebsitesRepository websitesRepository)
        {
            _websitesRepository = websitesRepository;
        }

        public WebsiteDetails Add(WebsiteDetails websiteDetails)
        {
            validateWebsiteDetails(websiteDetails);
            return _websitesRepository.Add(websiteDetails);
        }

        public bool Delete(int id)
        {
            return _websitesRepository.Delete(id);
        }

        public async Task<List<WebsiteDetails>> GetAll()
        {
            return await _websitesRepository.GetAll();
        }

        public WebsiteDetails Get(int id)
        {
            return _websitesRepository.Get(id);
        }

        public WebsiteDetails Save(WebsiteDetails websiteDetails)
        {
            validateWebsiteDetails(websiteDetails);
            return _websitesRepository.Save(websiteDetails);
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
