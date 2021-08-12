using news_scrapper.application.Interfaces;
using System;

namespace news_scrapper.infrastructure
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
