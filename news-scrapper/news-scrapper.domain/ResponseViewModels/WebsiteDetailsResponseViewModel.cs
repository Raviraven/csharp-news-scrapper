using news_scrapper.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.domain.ResponseViewModels
{
    public class WebsiteDetailsResponseViewModel
    {
        public WebsiteDetailsResponseViewModel()
        {
            WebsiteDetails = new();
            ErrorMessages = new();
        }

        public List<WebsiteDetails> WebsiteDetails { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
