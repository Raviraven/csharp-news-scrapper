using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.domain.DBModels
{
    public class ArticleDb
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateScrapped { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public WebsiteDetailsDb WebsiteDetails { get; set; }
    }
}
