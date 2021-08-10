using Moq;
using news_scrapper.application.Repositories;
using news_scrapper.infrastructure.Data;

namespace news_scrapper.infrastructure.unit_tests.WebsiteDetailsServiceTests
{
    public class BaseTest
    {
        protected Mock<IWebsitesRepository> _websitesRepository { get; set; }

        protected WebsiteDetailsService _sut { get; set; }

        public BaseTest()
        {
            _websitesRepository = new Mock<IWebsitesRepository>();

            _sut = new WebsiteDetailsService(_websitesRepository.Object);
        }
    }
}
