using System;

namespace news_scrapper.domain.Exceptions
{
    public class DevException : Exception
    {
        public DevException() : base()
        {
        }

        public DevException(string message) : base(message)
        {
        }
    }
}
