using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.domain.DBModels
{
    public class UserDb
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        
        public string PasswordHash { get; set; }

        
        public List<RefreshTokenDb> RefreshTokens { get; set; }
    }
}