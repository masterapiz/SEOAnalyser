using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEOAnalyser.Models
{
    public class Result
    {
        public bool isURL { get; set; }
        public string filteredText { get; set; } = string.Empty;
        public List<RepeatedWords> stopWordInContent { get; set; } = new List<RepeatedWords>();
        public List<RepeatedWords> stopWordInMeta { get; set; } = new List<RepeatedWords>();
        public int numberExternalLinks { get; set; } = 0;

        public bool Empty
        {
            get
            {
                return (filteredText == string.Empty &&
                       stopWordInContent.Count == 0 &&
                       stopWordInMeta.Count ==0 &&
                       numberExternalLinks==0);
            }
        }
    }
}