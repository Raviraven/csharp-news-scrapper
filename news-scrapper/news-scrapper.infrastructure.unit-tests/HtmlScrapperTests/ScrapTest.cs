using Bogus;
using FluentAssertions;
using Moq;
using news_scrapper.application.Interfaces;
using news_scrapper.domain.Models;
using news_scrapper.resources;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace news_scrapper.infrastructure.unit_tests.HtmlScrapperTests
{
    public class ScrapTest
    {
        protected HtmlScrapper _sut { get; set; }
        private Mock<IDateTimeProvider> _dateTimeProvider { get; set; }

        public ScrapTest()
        {
            _sut = new HtmlScrapper(_dateTimeProvider.Object);
        }

        [Fact]
        public void should_return_articles()
        {
            string newsTitle = "just the test news title";
            string url = "https://test.test/";
            string newsUrl = "/news-url";
            string description = "test news description";
            string imageUrl = "/image urll";

            string mainNodeId = "test-news-id";
            WebsiteDetails details = createNewWebDetails(url, mainNodeId);
            string rawHtml = generateRawHtml(details, mainNodeId, newsTitle, newsUrl, description, imageUrl);

            List<Article> expectedArticles = new()
            {
                new()
                {
                    Title = $"{newsTitle}",
                    Url = $"{url[0..^1]}{newsUrl}",
                    Description = $"{description}",
                    ImageUrl = $"{url[0..^1]}{imageUrl}",
                }
            };

            (var articles, var errors) = _sut.Scrap(details, rawHtml);
            
            errors.Should().BeEmpty();
            articles.Should().BeEquivalentTo(expectedArticles);
        }

        [Fact]
        public void should_return_article_with_just_title_when_cant_find_nodes_with_article_data()
        {
            string newsTitle = "just the test news title";
            string rawHtml = $"<wrapper id=\"test-news-id\"><testTag></testTag><testTag class=\"test-news-class\">"+
                $"<h1 class=\"news-title\">{newsTitle}<h1></testTag></wrapper>";

            WebsiteDetails details = new()
            {
                MainNodeXPathToNewsContainer = "//*[@id=\"test-news-id\"]",
                NewsNodeTag = "testTag",
                NewsNodeClass = "test-news-class",
                TitleNodeTag = "h1",
                TitleNodeClass = "news-title",
            };

            List<Article> expectedArticles = new()
            {
                new()
                {
                    Title = $"{newsTitle}",
                    Url = "",
                    Description = "",
                    ImageUrl = "",
                }
            };

            (var articles, var errors) = _sut.Scrap(details, rawHtml);

            articles.Should().BeEquivalentTo(expectedArticles);
            errors.Should().BeEmpty();
        }

        [Fact]
        public void should_return_empty_articles_when_cant_get_main_node_by_xpath()
        {
            string rawHtml = "<wrapper id=\"test-news-id\"><testTag></wrapper>";

            WebsiteDetails details = new()
            {
                MainNodeXPathToNewsContainer = "//*[@id=\"totally-wrong-id\"]",
            };

            List<string> expectedErrors = new()
            {
                string.Format(ApiResponses.CannotGetMainNewsNodeByXpath, details.MainNodeXPathToNewsContainer)
            };

            (var articles, var errors) = _sut.Scrap(details, rawHtml);

            articles.Should().BeEmpty();
            errors.Should().BeEquivalentTo(expectedErrors);
        }

        [Fact]
        public void should_return_empty_articles_when_cant_get_news_nodes()
        {
            string rawHtml = "<wrapper id=\"test-news-id\"><testTag></testTag><testTag class=\"test-news-class\"></testTag></wrapper>";

            WebsiteDetails details = new()
            {
                MainNodeXPathToNewsContainer = "//*[@id=\"test-news-id\"]",
                NewsNodeTag = "testTag",
                NewsNodeClass = "wrong-class"
            };

            (var articles, var errors) = _sut.Scrap(details, rawHtml);

            articles.Should().BeEmpty();
            errors.Should().BeEmpty();
        }

        [Fact]
        public void should_return_error_messages_when_cant_get_title_node()
        {
            string rawHtml = "<wrapper id=\"test-news-id\"><testTag></testTag><testTag class=\"test-news-class\"></testTag></wrapper>";

            WebsiteDetails details = new()
            {
                MainNodeXPathToNewsContainer = "//*[@id=\"test-news-id\"]",
                NewsNodeTag = "testTag",
                NewsNodeClass = "test-news-class"
            };

            List<string> expectedErrors = new()
            {
                ApiResponses.CannotGetTitleFromMainNode
            };

            (var articles, var errors) = _sut.Scrap(details, rawHtml);

            articles.Should().BeEmpty();
            errors.Should().BeEquivalentTo(expectedErrors);
        }

        [Fact]
        public void should_return_found_urls_when_root_url_is_null()
        {
            string newsTitle = "just the test news title";
            string newsUrl = "/news-url";
            string description = "test news description";
            string imageUrl = "/image urll";

            string mainNodeId = "test-news-id";

            WebsiteDetails details = createNewWebDetails(null, mainNodeId);

            string rawHtml = generateRawHtml(details, mainNodeId, newsTitle, newsUrl, description, imageUrl);

            List<Article> expectedArticles = new()
            {
                new()
                {
                    Title = $"{newsTitle}",
                    Url = $"{newsUrl}",
                    Description = $"{description}",
                    ImageUrl = $"{imageUrl}",
                }
            };

            (var articles, var errors) = _sut.Scrap(details, rawHtml);

            articles.Should().BeEquivalentTo(expectedArticles);
            errors.Should().BeEmpty();
        }

        [Fact]
        public void should_return_found_urls_when_root_url_is_included()
        {
            string newsTitle = "just the test news title";
            string url = "https://test.test/";
            string newsUrl = $"{url}news-url";
            string description = "test news description";
            string imageUrl = $"{url}image urll";

            string mainNodeId = "test-news-id";

            WebsiteDetails details = createNewWebDetails(url, mainNodeId);
            string rawHtml = generateRawHtml(details, mainNodeId, newsTitle, newsUrl, description, imageUrl);

            List<Article> expectedArticles = new()
            {
                new()
                {
                    Title = $"{newsTitle}",
                    Url = $"{newsUrl}",
                    Description = $"{description}",
                    ImageUrl = $"{imageUrl}",
                }
            };

            (var articles, var errors) = _sut.Scrap(details, rawHtml);

            articles.Should().BeEquivalentTo(expectedArticles);
            errors.Should().BeEmpty();
        }

        [Fact]
        public void should_not_remove_last_char_from_url_when_it_is_not_slash()
        {
            string newsTitle = "just the test news title";
            string url = "https://test.test";
            string newsUrl = "/news-url";
            string description = "test news description";
            string imageUrl = "/image urll";

            string mainNodeId = "test-news-id";

            WebsiteDetails details = createNewWebDetails(url, mainNodeId);
            string rawHtml = generateRawHtml(details, mainNodeId, newsTitle, newsUrl, description, imageUrl);

            List<Article> expectedArticles = new()
            {
                new()
                {
                    Title = $"{newsTitle}",
                    Url = $"{url}{newsUrl}",
                    Description = $"{description}",
                    ImageUrl = $"{url}{imageUrl}",
                }
            };

            (var articles, var errors) = _sut.Scrap(details, rawHtml);

            articles.Should().BeEquivalentTo(expectedArticles);
            errors.Should().BeEmpty();
        }



        private WebsiteDetails createNewWebDetails(string url, string mainNodeId)
        {
            return new Faker<WebsiteDetails>()
                .RuleFor(n => n.MainNodeXPathToNewsContainer, b => $"//*[@id=\"{mainNodeId}\"]")
                .RuleFor(n => n.NewsNodeTag, b => b.Name.FirstName() + "-news-tag")
                .RuleFor(n => n.NewsNodeClass, b => b.Name.FirstName() + "-news-class")
                .RuleFor(n => n.TitleNodeTag, b => b.Name.FirstName() + "-title-tag")
                .RuleFor(n => n.TitleNodeClass, b => b.Name.FirstName() + "-title-class")
                .RuleFor(n => n.DescriptionNodeTag, b => b.Name.FirstName() + "-description-tag")
                .RuleFor(n => n.DescriptionNodeClass, b => b.Name.FirstName() + "-description-class")
                .RuleFor(n => n.ImgNodeClass, b => b.Name.FirstName() + "-img-class")
                .RuleFor(n => n.Url, b => url)
                .Generate();
        }

        private static string generateRawHtml(WebsiteDetails websiteDetail, string mainNodeId,
            string newsTitle, string newsUrl, string description, string imageUrl)
        {
            string websiteDetailHtml = generateHtmlForWebsiteDetail(newsTitle, newsUrl, description, imageUrl, websiteDetail);
            return $"<wrapper id=\"{mainNodeId}\">{websiteDetailHtml}</wrapper>";
        }

        private static string generateRawHtml(List<WebsiteDetails> websiteDetails, string mainNodeId,
            string newsTitle, string newsUrl, string description, string imageUrl)
        {
            StringBuilder sb = new();
            sb.Append($"<wrapper id=\"{mainNodeId}\">");

            foreach (var websiteDetail in websiteDetails)
            {
                string html = generateHtmlForWebsiteDetail(newsTitle, newsUrl, description, imageUrl, websiteDetail);

                sb.Append(html);
            }

            sb.Append("</wrapper>");

            return sb.ToString();
        }

        private static string generateHtmlForWebsiteDetail(string newsTitle, string newsUrl, string description, string imageUrl, WebsiteDetails websiteDetail)
        {
            string newsNodeTag = websiteDetail.NewsNodeTag;
            string newsNodeClass = websiteDetail.NewsNodeClass;
            string titleNodeTag = websiteDetail.TitleNodeTag;
            string titleNodeClass = websiteDetail.TitleNodeClass;
            string descriptionNodeTag = websiteDetail.DescriptionNodeTag;
            string descriptionNodeClass = websiteDetail.DescriptionNodeClass;
            string imgNodeClass = websiteDetail.ImgNodeClass;

            string html = $"<{newsNodeTag}></{newsNodeTag}><{newsNodeTag} class=\"{newsNodeClass}\">" +
            $"<{titleNodeTag} class=\"{titleNodeClass}\"><a href=\"{newsUrl}\">{newsTitle}</a></{titleNodeTag}>" +
            $"<{descriptionNodeTag} class=\"{descriptionNodeClass}\">{description}</{descriptionNodeTag}>" +
            $"<img class=\"{imgNodeClass}\" src=\"{imageUrl}\" /></{newsNodeTag}>";
            return html;
        }
    }
}
