using news_scrapper.application.Interfaces;
using System.Security.Cryptography;

namespace news_scrapper.infrastructure
{
    public class RandomCryptoBytesGenerator : IRandomCryptoBytesGenerator
    {
        public byte[] Get(int count)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return randomBytes;
        }
    }
}
