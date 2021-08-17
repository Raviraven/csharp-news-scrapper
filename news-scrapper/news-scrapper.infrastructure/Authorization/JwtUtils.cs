using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using news_scrapper.application.Authorization;
using news_scrapper.application.Interfaces;
using news_scrapper.domain;
using news_scrapper.domain.Exceptions;
using news_scrapper.domain.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace news_scrapper.infrastructure.Authorization
{
    public class JwtUtils : IJwtUtils
    {
        private readonly AppSettings _appSettings;
        private IDateTimeProvider _dateTimeProvider;
        private IRandomCryptoBytesGenerator _randomCryptoBytesGenerator;

        public JwtUtils(IOptions<AppSettings> appSettings,
            IDateTimeProvider dateTimeProvider, 
            IRandomCryptoBytesGenerator randomCryptoBytesGenerator)
        {
            _appSettings = appSettings.Value;
            _dateTimeProvider = dateTimeProvider;
            _randomCryptoBytesGenerator = randomCryptoBytesGenerator;
        }

        public string GenerateJwtToken(User user)
        {
            validateAppSettingsSecret();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = _dateTimeProvider.Now.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            // generate token that is valid for 7 days
            var randomBytes = _randomCryptoBytesGenerator.Get(64);

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = _dateTimeProvider.Now.AddDays(7),
                Created = _dateTimeProvider.Now,
                CreatedByIp = ipAddress
            };

            return refreshToken;
        }

        public int? ValidateJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            validateAppSettingsSecret();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            try
            {
                var validatedToken = validateToken(token, tokenHandler, key);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                return userId;
            }
            catch
            {
                return null;
            }
        }

        private SecurityToken validateToken(string token, JwtSecurityTokenHandler tokenHandler, byte[] key)
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            return validatedToken;
        }

        private void validateAppSettingsSecret()
        {
            if (_appSettings.Secret is null || _appSettings.Secret.Length < 16)
                throw new DevException("Length of the secret key < 16");
        }

    }
}
