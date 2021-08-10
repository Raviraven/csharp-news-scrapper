﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_scrapper.domain
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

        public void UpdateValues(WebsiteDetails website)
        {
            this.Url = website.Url;
            this.MainNodeXPathToNewsContainer = website.MainNodeXPathToNewsContainer;
            this.NewsNodeTag = website.NewsNodeTag;
            this.NewsNodeClass = website.NewsNodeClass;
            this.TitleNodeTag = website.TitleNodeTag;
            this.TitleNodeClass = website.TitleNodeClass;
            this.DescriptionNodeTag = website.DescriptionNodeTag;
            this.DescriptionNodeClass = website.DescriptionNodeClass;
            this.ImgNodeClass = website.ImgNodeClass;
            this.Category = website.Category;
        }
    }
}
