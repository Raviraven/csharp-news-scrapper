using HtmlAgilityPack;
using news_scrapper.application.Interfaces;
using news_scrapper.domain.Models;
using news_scrapper.resources;
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
        private IDateTimeProvider _dateTimeProvider { get; set; }

        public HtmlScrapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public (List<Article>, List<string>) Scrap(WebsiteDetails website, string rawHtml)
        {
            List<Article> articles = new();
            List<string> errors = new();

            try
            {
                var mainNode = getMainNode(rawHtml, website.MainNodeXPathToNewsContainer);
                var newsNodes = getNewsNodes(mainNode, website.NewsNodeTag, website.NewsNodeClass);

                for (int i = 0; i < newsNodes.Count; i++)
                {
                    HtmlNode titleNode = getTitleNode(website.TitleNodeTag, website.TitleNodeClass, newsNodes[i]);
                    string title = getTitle(titleNode);
                    string directUrlToNews = getNewsUrl(titleNode);
                    string image = getImageUrl(website.ImgNodeClass, newsNodes[i]);
                    string description = getDescription(website.DescriptionNodeTag, website.DescriptionNodeClass, newsNodes[i]);

                    articles.Add(new()
                    {
                        Title = title,
                        Url = getDirectUrl(directUrlToNews, website.Url),
                        ImageUrl = getDirectUrl(image, website.Url),
                        Description = description,
                        DateScrapped = _dateTimeProvider.Now,
                        WebsiteDetailsId = website.Id
                    });
                }
            }
            catch(Exception ex)
            {
                errors.Add(ex.Message);
            }

            return (articles, errors);
        }

        private string getTitle(HtmlNode titleNode)
        {
            return decodeHtmlString(removeTabsAndNewLines(titleNode?.InnerText ?? ""));
        }

        private string decodeHtmlString(string stringToDecode)
        {
            return System.Web.HttpUtility.HtmlDecode(stringToDecode);
        }

        private string removeTabsAndNewLines(string str)
        {
            str = str.Replace("\n", string.Empty);
            str = str.Replace("\t", string.Empty);
            return str.Trim();
        }

        private HtmlNode getMainNode(string rawHtml, string xPath)
        {
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(rawHtml);

            var mainNodeCollection = htmlDoc.DocumentNode.SelectNodes(xPath);
            
            if (mainNodeCollection is null)
            {
                throw new Exception(string.Format(ApiResponses.CannotGetMainNewsNodeByXpath, xPath));
            }

            return mainNodeCollection.First();
        }

        private List<HtmlNode> getNewsNodes(HtmlNode mainNode, string htmlElements, string htmlElementsClass)
        {
            return mainNode.Descendants(htmlElements).Where(n => n.GetAttributeValue("class", "").Contains(htmlElementsClass)).ToList();
        }

        private string getDescription(string descriptionNodeTag, string descriptionNodeClass, HtmlNode newsNode)
        {
            var descriptionNode = newsNode.Descendants(descriptionNodeTag).FirstOrDefault(n => n.GetAttributeValue("class", "").Contains(descriptionNodeClass));

            if (descriptionNode is null)
                return "";
            
            return decodeHtmlString(removeTabsAndNewLines(descriptionNode.InnerText));
        }

        private string getImageUrl(string imgNodeClass, HtmlNode newsNode)
        {
            var images = newsNode.Descendants("img").FirstOrDefault(n => n.GetAttributeValue("class", "").Contains(imgNodeClass));
            
            if(images is null)
                return "";

            return images.GetAttributeValue("src", "");
        }

        private string getNewsUrl(HtmlNode titleNode)
        {
            var newsUrl = titleNode.Descendants("a").FirstOrDefault()?.GetAttributeValue("href", "");

            if (newsUrl is null)
                return "";

            return newsUrl;
        }

        private HtmlNode getTitleNode(string titleNodeTag, string titleNodeClass, HtmlNode newsNode)
        {
            var titleNode = newsNode.Descendants(titleNodeTag).FirstOrDefault(n => n.GetAttributeValue("class", "").Contains(titleNodeClass));
            
            if(titleNode is null)
            {
                throw new Exception(ApiResponses.CannotGetTitleFromMainNode);
            }
            
            return titleNode;
        }

        private string getDirectUrl(string url, string siteUrl)
        {
            if (siteUrl is null)
                return url;

            if (!url.Contains(siteUrl))
                return concatenateUrls(url, siteUrl);

            return url;
        }

        private static string concatenateUrls(string url, string siteUrl)
        {
            if(siteUrl.EndsWith("/"))
                return $"{siteUrl[0..^1]}{url}";

            return $"{siteUrl}{url}";
        }
    }
}
