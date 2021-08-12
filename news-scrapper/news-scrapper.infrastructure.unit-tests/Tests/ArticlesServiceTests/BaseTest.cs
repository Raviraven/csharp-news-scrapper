using AutoMapper;
using Moq;
using news_scrapper.application.Data;
using news_scrapper.application.Repositories;
using news_scrapper.application.UnitsOfWork;
using news_scrapper.domain.DBModels;
using news_scrapper.infrastructure.Data;

namespace news_scrapper.infrastructure.unit_tests.Tests.ArticlesServiceTests
{
    public class BaseTest
    {
        protected Mock<IPagesScrapperService> _pagesScrapperService { get; set; }
        protected Mock<IArticlesUnitOfWork> _articlesUnitOfWork { get; set; }
        protected Mock<IRepository<ArticleDb>>  _articlesRepository { get; set; }
        protected Mock<IMapper> _mapper { get; set; }

        protected ArticlesService _sut;

        public BaseTest()
        {
            _pagesScrapperService = new Mock<IPagesScrapperService>();
            _articlesUnitOfWork = new Mock<IArticlesUnitOfWork>();
            _mapper = new Mock<IMapper>();

            _articlesRepository = new Mock<IRepository<ArticleDb>>();

            _articlesUnitOfWork.Setup(n => n.Articles).Returns(_articlesRepository.Object);

            _sut = new ArticlesService(_pagesScrapperService.Object,
                _articlesUnitOfWork.Object,
                _mapper.Object);
        }
    }
}
