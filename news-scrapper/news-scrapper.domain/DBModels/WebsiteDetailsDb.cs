using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace news_scrapper.domain.DBModels
{
    public class WebsiteDetailsDb
    {
        [Key]
        public int id { get; set; }
        public string Url { get; set; }
        public string MainNodeXPathToNewsContainer { get; set; }
        public string NewsNodeTag { get; set; }
        public string NewsNodeClass { get; set; }
        public string TitleNodeTag { get; set; }
        public string TitleNodeClass { get; set; }
        public string DescriptionNodeTag { get; set; }
        public string DescriptionNodeClass { get; set; }
        public string ImgNodeClass { get; set; }
        public string Category { get; set; }

        public ICollection<ArticleDb> Articles { get; set; }
        public ICollection<CategoryDb> Categories { get; set; }
    }
}
