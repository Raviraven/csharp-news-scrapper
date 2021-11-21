using AutoMapper;
using Moq;
using news_scrapper.application.Repositories;
using news_scrapper.application.UnitsOfWork;
using news_scrapper.domain.DBModels;
using news_scrapper.infrastructure.Data;

namespace news_scrapper.infrastructure.unit_tests.Tests.CategoriesServiceTests
{
    public class BaseTest
    {
        protected Mock<IMapper> _mapper { get; set; }
        protected Mock<ICategoriesUnitOfWork> _categoriesUnitOfWork { get; set; }

        protected Mock<IRepository<CategoryDb>> _categories { get; set; }

        protected CategoriesService _sut;

        public BaseTest()
        {
            _mapper = new();
            _categoriesUnitOfWork = new();

            _categories = new();

            _categoriesUnitOfWork.Setup(n => n.Categories).Returns(_categories.Object);

            _sut = new CategoriesService(_mapper.Object, 
                _categoriesUnitOfWork.Object);
        }
    }
}
