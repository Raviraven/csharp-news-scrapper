namespace news_scrapper.application.Interfaces
{
    public interface IRandomCryptoBytesGenerator
    {
        byte[] Get(int count);
    }
}
