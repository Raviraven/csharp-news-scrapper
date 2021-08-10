using news_scrapper.domain;
using news_scrapper.domain.ResponseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.application.Data
{
    public interface IPagesScrapperService
    {
        Task<ArticlesResponseViewModel> ScrapAll();
    }
}
