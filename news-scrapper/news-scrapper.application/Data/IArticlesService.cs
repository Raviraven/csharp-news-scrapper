﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.application.Data
{
    public interface IArticlesService
    {
        Task<List<string>> Scrap();
    }
}