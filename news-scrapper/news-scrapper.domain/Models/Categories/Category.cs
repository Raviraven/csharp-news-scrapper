using news_scrapper.domain.Models.WebsiteDetails;
using System.Collections.Generic;

namespace news_scrapper.domain.Models.Categories
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<WebsiteDetailsCategory> Websites { get; set; }
    }
}
