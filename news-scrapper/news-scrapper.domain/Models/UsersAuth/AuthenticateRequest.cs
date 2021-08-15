using System.ComponentModel.DataAnnotations;

namespace news_scrapper.domain.Models.UsersAuth
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
