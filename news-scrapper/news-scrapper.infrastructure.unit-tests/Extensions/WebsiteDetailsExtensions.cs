using news_scrapper.domain.DBModels;
using news_scrapper.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.infrastructure.unit_tests
{
    public static class WebsiteDetailsExtensions
    {
        public static WebsiteDetails Map(this WebsiteDetailsDb website)
        {
            if (website is null) return null;

            return new()
            {
                Id = website.id,
                Url = website.Url,
                MainNodeXPathToNewsContainer = website.MainNodeXPathToNewsContainer,
                NewsNodeTag = website.NewsNodeTag,
                NewsNodeClass = website.NewsNodeClass,
                TitleNodeTag = website.TitleNodeTag,
                TitleNodeClass = website.TitleNodeClass,
                DescriptionNodeTag = website.DescriptionNodeTag,
                DescriptionNodeClass = website.DescriptionNodeClass,
                ImgNodeClass = website.ImgNodeClass,
                Category = website.Category
            };
        }

        public static List<WebsiteDetails> Map(this List<WebsiteDetailsDb> websites)
        {
            if (websites is null)
                return null;

            List<WebsiteDetails> result = new();

            foreach (var website in websites)
            {
                result.Add(new()
                {
                    Id = website.id,
                    Url = website.Url,
                    MainNodeXPathToNewsContainer = website.MainNodeXPathToNewsContainer,
                    NewsNodeTag = website.NewsNodeTag,
                    NewsNodeClass = website.NewsNodeClass,
                    TitleNodeTag = website.TitleNodeTag,
                    TitleNodeClass = website.TitleNodeClass,
                    DescriptionNodeTag = website.DescriptionNodeTag,
                    DescriptionNodeClass = website.DescriptionNodeClass,
                    ImgNodeClass = website.ImgNodeClass,
                    Category = website.Category
                });
            }

            return result;
        }

        public static WebsiteDetailsDb Map(this WebsiteDetails website)
        {
            if (website is null) return null;

            return new()
            {
                id = website.Id,
                Url = website.Url,
                MainNodeXPathToNewsContainer = website.MainNodeXPathToNewsContainer,
                NewsNodeTag = website.NewsNodeTag,
                NewsNodeClass = website.NewsNodeClass,
                TitleNodeTag = website.TitleNodeTag,
                TitleNodeClass = website.TitleNodeClass,
                DescriptionNodeTag = website.DescriptionNodeTag,
                DescriptionNodeClass = website.DescriptionNodeClass,
                ImgNodeClass = website.ImgNodeClass,
                Category = website.Category
            };
        }


        public static List<WebsiteDetailsDb> Map(this List<WebsiteDetails> websites)
        {
            if (websites is null)
                return null;

            List<WebsiteDetailsDb> result = new();

            foreach (var website in websites)
            {
                result.Add(new()
                {
                    id = website.Id,
                    Url = website.Url,
                    MainNodeXPathToNewsContainer = website.MainNodeXPathToNewsContainer,
                    NewsNodeTag = website.NewsNodeTag,
                    NewsNodeClass = website.NewsNodeClass,
                    TitleNodeTag = website.TitleNodeTag,
                    TitleNodeClass = website.TitleNodeClass,
                    DescriptionNodeTag = website.DescriptionNodeTag,
                    DescriptionNodeClass = website.DescriptionNodeClass,
                    ImgNodeClass = website.ImgNodeClass,
                    Category = website.Category
                });
            }

            return result;
        }
    }
}
