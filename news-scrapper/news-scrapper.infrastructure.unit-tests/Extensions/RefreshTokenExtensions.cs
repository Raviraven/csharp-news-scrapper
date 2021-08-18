using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.infrastructure.unit_tests
{
    public static class RefreshTokenExtensions
    {
        public static RefreshToken Map(this RefreshTokenDb refreshToken)
        {
            if (refreshToken is null) return null;

            return new() 
            {
                Id = refreshToken.Id ,
                Token = refreshToken.Token ,
                Expires = refreshToken.Expires,
                Created = refreshToken.Created,
                CreatedByIp = refreshToken.CreatedByIp,
                Revoked = refreshToken.Revoked,
                RevokedByIp = refreshToken.RevokedByIp,
                ReplacedByToken = refreshToken.ReplacedByToken,
                ReasonRevoked = refreshToken.ReasonRevoked
            };
        }

        public static RefreshToken GetCopy(this RefreshToken refreshToken)
        {
            return new()
            {
                Id = refreshToken.Id,
                Token = refreshToken.Token,
                Expires = refreshToken.Expires,
                Created = refreshToken.Created,
                CreatedByIp = refreshToken.CreatedByIp,
                Revoked = refreshToken.Revoked,
                RevokedByIp = refreshToken.RevokedByIp,
                ReplacedByToken = refreshToken.ReplacedByToken,
                ReasonRevoked = refreshToken.ReasonRevoked
            };
        }

        public static List<RefreshToken> Map(this List<RefreshTokenDb> refreshTokens)
        {
            if (refreshTokens is null) return null;

            List<RefreshToken> result = new();

            foreach (var refreshToken in refreshTokens)
            {
                result.Add(new()
                {
                    Id = refreshToken.Id,
                    Token = refreshToken.Token,
                    Expires = refreshToken.Expires,
                    Created = refreshToken.Created,
                    CreatedByIp = refreshToken.CreatedByIp,
                    Revoked = refreshToken.Revoked,
                    RevokedByIp = refreshToken.RevokedByIp,
                    ReplacedByToken = refreshToken.ReplacedByToken,
                    ReasonRevoked = refreshToken.ReasonRevoked
                });
            }
            return result;
        }

        public static RefreshTokenDb Map(this RefreshToken refreshToken)
        {
            if (refreshToken is null) return null;

            return new()
            {
                Id = refreshToken.Id,
                Token = refreshToken.Token,
                Expires = refreshToken.Expires,
                Created = refreshToken.Created,
                CreatedByIp = refreshToken.CreatedByIp,
                Revoked = refreshToken.Revoked,
                RevokedByIp = refreshToken.RevokedByIp,
                ReplacedByToken = refreshToken.ReplacedByToken,
                ReasonRevoked = refreshToken.ReasonRevoked
            };
        }

        public static List<RefreshTokenDb> Map(this List<RefreshToken> refreshTokens)
        {
            if (refreshTokens is null) return null;

            List<RefreshTokenDb> result = new();

            foreach (var refreshToken in refreshTokens)
            {
                result.Add(new()
                {
                    Id = refreshToken.Id,
                    Token = refreshToken.Token,
                    Expires = refreshToken.Expires,
                    Created = refreshToken.Created,
                    CreatedByIp = refreshToken.CreatedByIp,
                    Revoked = refreshToken.Revoked,
                    RevokedByIp = refreshToken.RevokedByIp,
                    ReplacedByToken = refreshToken.ReplacedByToken,
                    ReasonRevoked = refreshToken.ReasonRevoked
                });
            }
            return result;
        }
    }
}
