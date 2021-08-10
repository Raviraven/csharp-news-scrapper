using System;

namespace news_scrapper.domain.Exceptions
{
    public class InvalidWebsiteDetailsException : Exception
    {
        public InvalidWebsiteDetailsException() : base()
        {
        }

        public InvalidWebsiteDetailsException(string message) : base(message)
        {
        }
    }
}
