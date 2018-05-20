using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SEOAnalyser.Models
{
    public class SearchModel
    {
        public string textSearch { get; set; }        
        public bool isRepeat { get; set; }
        public bool isRepeatMeta { get; set; }
        public bool isExternalLink { get; set; }
    }
}