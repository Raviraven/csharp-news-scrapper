using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Options;
using news_scrapper.application.Authorization;
using news_scrapper.application.Data;
using news_scrapper.application.UnitsOfWork;
using news_scrapper.domain;
using news_scrapper.domain.Models;
using news_scrapper.domain.Models.UsersAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using news_scrapper.domain.DBModels;
using news_scrapper.application.Interfaces;

namespace news_scrapper.infrastructure.Data
{
    public class UserService : IUserService
    {
        private IUsersUnitOfWork _usersUnitOfWork { get; set; }
        private IJwtUtils _jwtUtils { get; set; }
        private readonly AppSettings _appSettings;
        private IMapper _mapper { get; set; }
        private IDateTimeProvider _dateTimeProvider { get; set; }

        public UserService(IUsersUnitOfWork usersUnitOfWork,
            IJwtUtils jwtUtils,
            IOptions<AppSettings> appSettings, 
            IMapper mapper, 
            IDateTimeProvider dateTimeProvider)
        {
            _usersUnitOfWork = usersUnitOfWork;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }


        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var user = _usersUnitOfWork.Users.Get(
                    filter: n=>n.Username == model.Username
                )
                .SingleOrDefault();

            if (user is null || !BCryptNet.Verify(model.Password, user.PasswordHash))
                throw new Exception("Username or password is incorrect");

            var mappedUser = _mapper.Map<User>(user);

            var jwtToken = _jwtUtils.GenerateJwtToken(mappedUser);
            var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);

            mappedUser.RefreshTokens.Add(refreshToken);

            removeOldRefreshTokens(mappedUser);

            _usersUnitOfWork.Users.Update(_mapper.Map<UserDb>(mappedUser));
            _usersUnitOfWork.Commit();

            return new AuthenticateResponse(mappedUser, jwtToken, refreshToken.Token);
        }

        public IEnumerable<User> GetAll()
        {
            return _mapper.Map<List<User>>(_usersUnitOfWork.Users.Get());
        }

        public User GetById(int id)
        {
            var user = _usersUnitOfWork.Users.GetById(id);
            if (user is null)
                throw new KeyNotFoundException("User not found");

            return _mapper.Map<User>(user);
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var user = getUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(n => n.Token == token);

            if (refreshToken.IsRevoked)
            {
                //revoke all descendant tokens in case this token has been compromised
                revokeDescendantRefreshTokens(refreshToken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                _usersUnitOfWork.Users.Update(_mapper.Map<UserDb>(user));
                _usersUnitOfWork.Commit();
            }

            if (!refreshToken.IsActive)
                throw new Exception("Invalid token");

            var newRefreshToken = rotateRefreshToken(refreshToken, ipAddress);
            user.RefreshTokens.Add(newRefreshToken);

            removeOldRefreshTokens(user);

            _usersUnitOfWork.Users.Update(_mapper.Map<UserDb>(user));
            _usersUnitOfWork.Commit();

            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
        }

        public void RevokeToken(string token, string ipAddress)
        {
            var user = getUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(n => n.Token == token);

            if (!refreshToken.IsActive)
                throw new Exception("Invalid token");

            revokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
            _usersUnitOfWork.Users.Update(_mapper.Map<UserDb>(user));
            _usersUnitOfWork.Commit();
        }

        private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = _dateTimeProvider.Now;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }

        private User getUserByRefreshToken(string token)
        {
            var user = _usersUnitOfWork.Users.Get(
                    filter: n=>n.RefreshTokens.Any(t => t.Token == token)
                )
                .SingleOrDefault();

            if (user is null)
                throw new Exception("Invalid token");

            return _mapper.Map<User>(user);
        }

        private void revokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
        {
            if(!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(n => n.Token == refreshToken.ReplacedByToken);

                if (childToken.IsActive)
                    revokeRefreshToken(childToken, ipAddress, reason);
                else
                    revokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
            }
        }

        private RefreshToken rotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void removeOldRefreshTokens(User user)
        {
            user.RefreshTokens.RemoveAll( x=>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= _dateTimeProvider.Now);
        }

    }
}
