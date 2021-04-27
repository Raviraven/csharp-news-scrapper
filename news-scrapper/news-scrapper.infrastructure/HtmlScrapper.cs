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
        private HttpClient _httpclient { get; set; }

        public HtmlScrapper(HttpClient httpClient)
        {
            _httpclient = httpClient;
        }

        public async Task<List<Article>> Scrap(string url, string xPathToNewsContainer, string mainNodeTag, string mainNodeClass, string titleNodeTag, string titleNodeClass,
            string descriptionNodeTag, string descriptionNodeClass, string imgNodeClass)
        {
            List<Article> articles = new();

            var htmlString = await callUrl(url);
            var newsNodes = parseHtml(htmlString, xPathToNewsContainer, mainNodeTag, mainNodeClass);

            for (int i = 0; i < newsNodes.Count; i++)
            {
                HtmlNode titleNode = getTitleNode(titleNodeTag, titleNodeClass, newsNodes[i]);

                var title = titleNode.InnerText;
                string directUrlToNews = getNewsUrl(titleNode);
                string image = getImageUrl(imgNodeClass, newsNodes[i]);
                string description = getDescription(descriptionNodeTag, descriptionNodeClass, newsNodes[i]);

                articles.Add(new()
                {
                    Title = title,
                    Url = getDirectUrl(directUrlToNews, url),
                    ImageUrl = getDirectUrl(image, url),
                    Description = description
                });
            }

            return await Task.FromResult(articles);
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

        private async Task<string> callUrl(string url)
        {
            var response = await _httpclient.GetStringAsync(url);
            return response;
        }

        private List<HtmlNode> parseHtml(string html, string xPath, string htmlElements, string htmlElementsClass)
        {
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(html);

            var chosenElement = htmlDoc.DocumentNode
                .SelectNodes(xPath).First();

            var elementChildren = chosenElement.Descendants(htmlElements)
                .Where(n => n.GetAttributeValue("class", "").Contains(htmlElementsClass)).ToList();
            
            return elementChildren;
        }
    }
}
