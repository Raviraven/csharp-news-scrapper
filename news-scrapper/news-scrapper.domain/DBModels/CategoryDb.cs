using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace news_scrapper.domain.DBModels
{
    public class CategoryDb
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<WebsiteDetailsDb> Websites { get; set; }
    }
}
