﻿using BCryptNet = BCrypt.Net.BCrypt;
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
using news_scrapper.resources;

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
            var userFromDb = _usersUnitOfWork.Users.Get(
                filter: n => n.Username == model.Username).SingleOrDefault();

            if (userFromDb is null || !BCryptNet.Verify(model.Password, userFromDb.PasswordHash))
                throw new Exception(ApiResponses.UsernameOrPasswordIsIncorrect);
            
            var user = _mapper.Map<User>(userFromDb);

            var jwtToken = _jwtUtils.GenerateJwtToken(user);
            var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);

            user.RefreshTokens.Add(refreshToken);

            removeOldRefreshTokens(user);

            _mapper.Map(user, userFromDb);

            _usersUnitOfWork.Users.Update(userFromDb);
            _usersUnitOfWork.Commit();

            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
        }

        public IEnumerable<User> GetAll()
        {
            return _mapper.Map<List<User>>(_usersUnitOfWork.Users.Get());
        }

        public User GetById(int id)
        {
            var user = _usersUnitOfWork.Users.GetById(id);
            if (user is null)
                throw new KeyNotFoundException(ApiResponses.UserNotFound);

            return _mapper.Map<User>(user);
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var userFromDb = getUserDbByRefreshToken(token);

            var user = _mapper.Map<User>(userFromDb);

            var refreshToken = user.RefreshTokens.Single(n => n.Token == token);

            if (refreshToken.IsRevoked)
            {
                //revoke all descendant tokens in case this token has been compromised
                revokeDescendantRefreshTokens(refreshToken, user, ipAddress, string.Format(ApiResponses.AttemptedReuseOfRevokedToken, token));
                _usersUnitOfWork.Users.Update(_mapper.Map<UserDb>(user));
                _usersUnitOfWork.Commit();
            }

            if (!refreshToken.IsActive)
            {
                //_usersUnitOfWork.Users.Update(_mapper.Map<UserDb>(user));
                //_usersUnitOfWork.Commit();
                throw new Exception(ApiResponses.InvalidToken);
            }

            var newRefreshToken = rotateRefreshToken(refreshToken, ipAddress);
            user.RefreshTokens.Add(newRefreshToken);

            removeOldRefreshTokens(user);

            _mapper.Map(user, userFromDb);

            _usersUnitOfWork.Users.Update(userFromDb);
            _usersUnitOfWork.Commit();

            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
        }

        public void RevokeToken(string token, string ipAddress)
        {
            var userFromDb = getUserDbByRefreshToken(token);
            var user = _mapper.Map<User>(userFromDb);

            var refreshToken = user.RefreshTokens.Single(n => n.Token == token);

            if (!refreshToken.IsActive)
                throw new Exception(ApiResponses.InvalidToken);

            revokeRefreshToken(refreshToken, ipAddress, ApiResponses.RevokedWithoutReplacement);
            _mapper.Map(user, userFromDb);

            _usersUnitOfWork.Users.Update(userFromDb);
            _usersUnitOfWork.Commit();
        }

        private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = _dateTimeProvider.Now;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }

        private UserDb getUserDbByRefreshToken(string token)
        {
            var user = _usersUnitOfWork.Users.Get(
                    filter: n=>n.RefreshTokens.Any(t => t.Token == token)
                )
                .SingleOrDefault();

            if (user is null)
                throw new Exception(ApiResponses.InvalidToken);

            return user;
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
            revokeRefreshToken(refreshToken, ipAddress, ApiResponses.ReplacedByNewToken, newRefreshToken.Token);
            return newRefreshToken;
        }

        private void removeOldRefreshTokens(User user)
        {
            user.RefreshTokens.RemoveAll(x =>
               !x.IsActive &&
               x.Created.AddDays(_appSettings.RefreshTokenTTL) <= _dateTimeProvider.Now);
        }

    }
}
