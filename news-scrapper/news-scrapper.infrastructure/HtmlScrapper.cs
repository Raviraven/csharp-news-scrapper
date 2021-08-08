using HtmlAgilityPack;
using news_scrapper.application.Interfaces;
using news_scrapper.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.infrastructure
{
    public class HtmlScrapper : IHtmlScrapper
    {
        public async Task<List<Article>> Scrap(WebsiteDetails website, string rawHtml)
        {
            List<Article> articles = new();

            var newsNodes = parseHtmlIntoNodes(rawHtml, website.XPathToNewsContainer, website.MainNodeTag, website.MainNodeClass);

            for (int i = 0; i < newsNodes.Count; i++)
            {
                HtmlNode titleNode = getTitleNode(website.TitleNodeTag, website.TitleNodeClass, newsNodes[i]);

                var title = titleNode.InnerText;
                string directUrlToNews = getNewsUrl(titleNode);
                string image = getImageUrl(website.ImgNodeClass, newsNodes[i]);
                string description = getDescription(website.DescriptionNodeTag, website.DescriptionNodeClass, newsNodes[i]);

                articles.Add(new()
                {
                    Title = title,
                    Url = getDirectUrl(directUrlToNews, website.Url),
                    ImageUrl = getDirectUrl(image, website.Url),
                    Description = description
                });
            }

            return await Task.FromResult(articles);
        }


        private List<HtmlNode> parseHtmlIntoNodes(string rawHtml, string xPath, string htmlElements, string htmlElementsClass)
        {
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(rawHtml);

            var chosenElement = htmlDoc.DocumentNode
                .SelectNodes(xPath).First();

            var elementChildren = chosenElement.Descendants(htmlElements)
                .Where(n => n.GetAttributeValue("class", "").Contains(htmlElementsClass)).ToList();

            return elementChildren;
        }


        private string getDescription(string descriptionNodeTag, string descriptionNodeClass, HtmlNode newsNode)
        {
            return newsNode.Descendants(descriptionNodeTag).FirstOrDefault(n => n.GetAttributeValue("class", "").Contains(descriptionNodeClass)).InnerText;
        }

        private string getImageUrl(string imgNodeClass, HtmlNode newsNode)
        {
            return newsNode.Descendants("img").FirstOrDefault(n => n.GetAttributeValue("class", "").Contains(imgNodeClass)).GetAttributeValue("src", "");
        }

        private string getNewsUrl(HtmlNode titleNode)
        {
            return titleNode.Descendants("a").FirstOrDefault()?.GetAttributeValue("href", "");
        }

        private HtmlNode getTitleNode(string titleNodeTag, string titleNodeClass, HtmlNode newsNode)
        {
            return newsNode.Descendants(titleNodeTag).FirstOrDefault(n => n.GetAttributeValue("class", "").Contains(titleNodeClass));
        }

        private string getDirectUrl(string url, string siteUrl)
        {
            if (!url.Contains(siteUrl))
                return $"{siteUrl[0..^1]}{url}";

            return url;
        }
    }
}
