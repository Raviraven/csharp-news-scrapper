using AutoMapper;
using news_scrapper.application.Interfaces;
using news_scrapper.application.Repositories;
using news_scrapper.domain.DBModels;
using news_scrapper.domain.Exceptions;
using news_scrapper.domain.Models.WebsiteDetails;
using news_scrapper.resources;
using System.Collections.Generic;
using System.Linq;

namespace news_scrapper.infrastructure.Data
{
    public class WebsiteDetailsService : IWebsiteDetailsService
    {
        private IWebsitesRepository _websitesRepository { get; set; }
        private IRepository<CategoryDb> _categoriesRepository { get; set; }
        private IMapper _mapper { get; set; }

        public WebsiteDetailsService(IWebsitesRepository websitesRepository, IMapper mapper, 
            IRepository<CategoryDb> categoriesRepository)
        {
            _websitesRepository = websitesRepository;
            _mapper = mapper;
            _categoriesRepository = categoriesRepository;
        }

        public WebsiteDetails Add(WebsiteDetails websiteDetails)
        {
            validateWebsiteDetails(websiteDetails);

            var websiteDetailsDb = _mapper.Map<WebsiteDetailsDb>(websiteDetails);
            
            if (websiteDetails.Categories != null)
            {
                var chosenCategories = websiteDetails?.Categories.Select(n => n.Id);
                var chosenCategoriesDb = _categoriesRepository.Get(n => chosenCategories.Contains(n.Id));

                websiteDetailsDb.Categories = chosenCategoriesDb.ToList();
            }

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

        public List<WebsiteDetails> GetAll(int userId)
        {
            var result = _mapper.Map<List<WebsiteDetails>>(_websitesRepository.GetAll(userId));
            return result;
        }

        public WebsiteDetails Get(int id)
        {
            var result = _mapper.Map<WebsiteDetails>(_websitesRepository.GetWithCategories(id));
            return result;
        }

        public WebsiteDetails Save(WebsiteDetails websiteDetails)
        {
            validateWebsiteDetails(websiteDetails);

            var websiteDetailsAlreadyExisting = _websitesRepository.GetWithCategories(websiteDetails.Id);

            websiteDetailsAlreadyExisting.Categories.Clear();

            _mapper.Map(websiteDetails, websiteDetailsAlreadyExisting);

            if (websiteDetails.Categories != null)
            {
                var chosenCategories = websiteDetails.Categories.Select(n => n.Id);
                var chosenCategoriesDb = _categoriesRepository.Get(n => chosenCategories.Contains(n.Id));
                 
                websiteDetailsAlreadyExisting.Categories = chosenCategoriesDb.ToList();
            }

            _mapper.Map(_websitesRepository.Save(websiteDetailsAlreadyExisting), websiteDetails);

            _websitesRepository.Commit();

            return websiteDetails;
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

            if(websiteDetails.Categories != null)
            {
                var categoriesIds = websiteDetails.Categories.Select(n=>n.Id);
                
                if (categoriesIds.Count() != categoriesIds.Distinct().Count())
                    throw new InvalidWebsiteDetailsException(ApiResponses.WebsiteDetailsCategoriesCannotBeDuplicated);
            }

        }
    }
}
