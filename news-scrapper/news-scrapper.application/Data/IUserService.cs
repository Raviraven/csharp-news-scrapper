using news_scrapper.domain.Models;
using news_scrapper.domain.Models.UsersAuth;
using System.Collections.Generic;

namespace news_scrapper.application.Data
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        void RevokeToken(string token, string ipAddress);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
