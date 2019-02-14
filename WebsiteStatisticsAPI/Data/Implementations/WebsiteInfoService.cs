using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebsiteStatisticsAPI.Data.Interfaces;
using WebsiteStatisticsAPI.Models;

namespace WebsiteStatisticsAPI.Data
{
    public class WebsiteInfoService : IWebsiteInfoService
    {
        public WebsiteStats GetAllData(string url)
        {
            try
            {
                var ImageList = new List<string>();
                var WordFrequency = new Dictionary<string, int>();
                //Utility class to get HTML document from http
                var web = new HtmlWeb();
                //Load() Method download the specified HTML document from Internet resource.
                var doc = web.Load(url);
                var rootNode = doc.DocumentNode;
                var nodes = doc.DocumentNode.SelectNodes("//img");
                foreach (var src in nodes)
                {
                    if (src.Attributes["src"] != null)
                        ImageList.Add(src.Attributes["src"].Value);
                }
                ImageList = ImageList.Distinct().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                var text = string.Empty;
                //fileter for words in the document which are not in the script or style tags and normalize the space
                foreach (var node in doc.DocumentNode.SelectNodes("//*[not(self::script or self::style)]/text()[normalize-space()]"))
                {
                    text += " " + node.InnerText;
                }
                //Remove carriage return, newline characters and empty strings
                var wordsArray = Regex.Split(text.Replace("\r", "").Replace("\n", ""), @"\s+").Where(s => s != string.Empty);
                var TotalWordsCount = 0;
                if (wordsArray != null)
                {
                    TotalWordsCount = wordsArray.Count();
                    foreach (var item in wordsArray)
                    {
                        if (WordFrequency.ContainsKey(item))
                        {
                            WordFrequency[item]++;
                        }
                        else
                        {
                            WordFrequency.Add(item, 1);
                        }
                    }
                }

                var WebPageStats = new WebsiteStats
                {
                    Images = ImageList,
                    TotalWordsCount = TotalWordsCount,
                    WordFrequency = WordFrequency
                };
                return WebPageStats;
            }
            catch (Exception ex)
            {
                // TODO; log error
                return null;
            }
        }
    }
}