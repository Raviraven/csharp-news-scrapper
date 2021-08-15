namespace news_scrapper.domain
{
    public class AppSettings
    {
        public string Secret { get; set; }

        //refresh token time to live (in days), inactive tokens are
        //automatically deleted from db after that time
        public int RefreshTokenTTL { get; set; }
    }
}
