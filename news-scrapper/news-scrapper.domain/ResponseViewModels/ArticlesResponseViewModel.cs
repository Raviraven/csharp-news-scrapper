using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.domain.ResponseViewModels
{
    public class ArticlesResponseViewModel
    {
        public ArticlesResponseViewModel()
        {
            Articles = new();
            ErrorMessages = new();
        }

        public List<Article> Articles { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
