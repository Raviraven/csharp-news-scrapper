using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.domain
{
    public class WebsiteDetails
    {
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
            XPathToNewsContainer = xPathToNewsContainer;
            MainNodeTag = mainNodeTag;
            MainNodeClass = mainNodeClass;
            TitleNodeTag = titleNodeTag;
            TitleNodeClass = titleNodeClass;
            DescriptionNodeTag = descriptionNodeTag;
            DescriptionNodeClass = descriptionNodeClass;
            ImgNodeClass = imgNodeClass;
        }

        public string Url { get; set; }
        public string XPathToNewsContainer { get; set; }
        public string MainNodeTag { get; set; }
        public string MainNodeClass { get; set; }
        public string TitleNodeTag { get; set; }
        public string TitleNodeClass { get; set; }
        public string DescriptionNodeTag { get; set; }
        public string DescriptionNodeClass { get; set; }
        public string ImgNodeClass { get; set; }
    }
}
