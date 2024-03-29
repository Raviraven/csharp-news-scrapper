﻿using news_scrapper.application.Repositories;
using news_scrapper.domain.DBModels;

namespace news_scrapper.application.UnitsOfWork
{
    public interface ICategoriesUnitOfWork
    {
        IRepository<CategoryDb> Categories { get; }
        IRepository<WebsiteDetailsDb> WebsiteDetails { get; }
        void Commit();
    }
}
