using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using news_scrapper.application.Authorization;
using news_scrapper.application.Interfaces;
using news_scrapper.application.Repositories;
using news_scrapper.application.UnitsOfWork;
using news_scrapper.domain;
using news_scrapper.domain.DBModels;
using news_scrapper.infrastructure.Data;

namespace news_scrapper.infrastructure.unit_tests.Tests.UserServiceTests
{
    public class BaseTest
    {
        protected Mock<IUsersUnitOfWork> _usersUnitOfWork { get; set; }
        protected Mock<IRepository<UserDb>> _users { get; }
        protected Mock<IJwtUtils> _jwtUtils { get; set; }
        protected Mock<IMapper> _mapper { get; set; }
        protected Mock<IDateTimeProvider> _dateTimeProvider { get; set; }
        protected readonly IOptions<AppSettings> _appSettings;

        protected UserService _sut;

        protected const string IP_ADDRESS = "546.987.153.619";

        public BaseTest()
        {
            _usersUnitOfWork = new Mock<IUsersUnitOfWork>();
            _jwtUtils = new Mock<IJwtUtils>();
            _mapper = new Mock<IMapper>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();

            _appSettings = Options.Create(new AppSettings { RefreshTokenTTL = 1, Secret = "" });

            _users = new Mock<IRepository<UserDb>>();

            _usersUnitOfWork.Setup(n => n.Users).Returns(_users.Object);

            _sut = new UserService(_usersUnitOfWork.Object,
                _jwtUtils.Object,
                _appSettings,
                _mapper.Object,
                _dateTimeProvider.Object);
        }
    }
}
