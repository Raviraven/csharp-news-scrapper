using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using news_scrapper.application.Interfaces;
using news_scrapper.domain;
using news_scrapper.domain.Models;
using news_scrapper.infrastructure.Authorization;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace news_scrapper.infrastructure.unit_tests.Tests.AuthorizationTests
{
    public class BaseTest
    {
        protected IOptions<AppSettings> _appSettings;
        protected Mock<IDateTimeProvider> _dateTimeProvider;
        protected Mock<IRandomCryptoBytesGenerator> _randomCryptoBytesGenerator;

        protected const string SECRET_KEY = "just the secret!";

        protected JwtUtils _sut;

        public BaseTest()
        {
            _appSettings = Options.Create(new AppSettings { Secret = SECRET_KEY });
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _randomCryptoBytesGenerator = new Mock<IRandomCryptoBytesGenerator>();

            _sut = new JwtUtils(_appSettings, _dateTimeProvider.Object, _randomCryptoBytesGenerator.Object);
        }


        protected string generateToken(User user, DateTime now, string key)
        {
            var keyEncoded = Encoding.ASCII.GetBytes(key);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = now.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyEncoded), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
