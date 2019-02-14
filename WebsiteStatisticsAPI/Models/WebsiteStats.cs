using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteStatisticsAPI.Models
{
    public class WebsiteStats
    {
        public IList<string> Images { get; set; }
        public int TotalWordsCount { get; set; }
        public IDictionary<string, int> WordFrequency { get; set; }
    }
}