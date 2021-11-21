using news_scrapper.domain.Models.Categories;

namespace news_scrapper.domain.Models
{
    public class WebsiteDetails
    {
        public WebsiteDetails()
        {
        }

        public WebsiteDetails(string url,
            string xPathToNewsContainer,
            string mainNodeTag,
            string mainNodeClass,
            string titleNodeTag,
            string titleNodeClass,
            string descriptionNodeTag,
            string descriptionNodeClass,
            string imgNodeClass)
        {
            Url = url;
            MainNodeXPathToNewsContainer = xPathToNewsContainer;
            NewsNodeTag = mainNodeTag;
            NewsNodeClass = mainNodeClass;
            TitleNodeTag = titleNodeTag;
            TitleNodeClass = titleNodeClass;
            DescriptionNodeTag = descriptionNodeTag;
            DescriptionNodeClass = descriptionNodeClass;
            ImgNodeClass = imgNodeClass;
        }

        public int Id { get; set; }
        public string Url { get; set; } = "";
        public string MainNodeXPathToNewsContainer { get; set; } = "";
        public string NewsNodeTag { get; set; } = "";
        public string NewsNodeClass { get; set; } = "";
        public string TitleNodeTag { get; set; } = "";
        public string TitleNodeClass { get; set; } = "";
        public string DescriptionNodeTag { get; set; } = "";
        public string DescriptionNodeClass { get; set; } = "";
        public string ImgNodeClass { get; set; } = "";
        public string Category { get; set; } = "";

        public Category[] Categories { get; set; }

        public void UpdateValues(WebsiteDetails website)
        {
            Url = website.Url;
            MainNodeXPathToNewsContainer = website.MainNodeXPathToNewsContainer;
            NewsNodeTag = website.NewsNodeTag;
            NewsNodeClass = website.NewsNodeClass;
            TitleNodeTag = website.TitleNodeTag;
            TitleNodeClass = website.TitleNodeClass;
            DescriptionNodeTag = website.DescriptionNodeTag;
            DescriptionNodeClass = website.DescriptionNodeClass;
            ImgNodeClass = website.ImgNodeClass;
            Category = website.Category;
        }
    }
}
