using System.ComponentModel.DataAnnotations;

namespace news_scrapper.domain.Models.Categories
{
    public class CategoryAdd
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
