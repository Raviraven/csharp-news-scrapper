using System.Collections.Generic;

namespace news_scrapper.domain.Models.Categories
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<WebsiteDetails> Websites { get; set; }
    }
}
