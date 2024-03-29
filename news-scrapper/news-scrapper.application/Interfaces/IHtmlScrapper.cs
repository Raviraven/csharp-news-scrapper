﻿using news_scrapper.domain.Models;
using news_scrapper.domain.Models.WebsiteDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.application.Interfaces
{
    public interface IHtmlScrapper
    {
        (List<Article>, List<string>) Scrap(WebsiteDetails website, string rawHtml);
    }
}
