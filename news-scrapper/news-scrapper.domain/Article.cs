using System;

namespace news_scrapper.domain
{
    public class Article
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
