using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models;
using System.Collections.Generic;

namespace news_scrapper.infrastructure.unit_tests
{
    public static class UserExtensions
    {
        public static User Map(this UserDb user)
        {
            if (user is null)
                return null;

            return new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PasswordHash = user.PasswordHash,
                RefreshTokens = user.RefreshTokens.Map(),
                Username = user.Username
            };
        }

        public static List<User> Map(this List<UserDb> users)
        {
            if (users is null)
                return null;

            List<User> result = new();

            foreach (var user in users)
            {
                result.Add(new() {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PasswordHash = user.PasswordHash,
                    RefreshTokens = user.RefreshTokens.Map(),
                    Username = user.Username
                });
            }
            return result;
        }

        public static UserDb Map(this User user)
        {
            if (user is null)
                return null;

            return new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PasswordHash = user.PasswordHash,
                RefreshTokens = user.RefreshTokens.Map(),
                Username = user.Username
            };
        }


        public static List<UserDb> Map(this List<User> users)
        {
            if (users is null)
                return null;

            List<UserDb> result = new();

            foreach (var user in users)
            {
                result.Add(new()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PasswordHash = user.PasswordHash,
                    RefreshTokens = user.RefreshTokens.Map(),
                    Username = user.Username
                });
            }
            return result;
        }
    }
}
