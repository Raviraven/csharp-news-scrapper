using System;

namespace news_scrapper.domain.Models
{
    public class Article
    {
        public int Id { get; set; }
        public DateTime DateScrapped { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        //?
        public int WebsiteDetailsId { get; set; }
    }
}
