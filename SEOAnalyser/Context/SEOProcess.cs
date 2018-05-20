using Newtonsoft.Json;
using SEOAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;

namespace SEOAnalyser.Context
{
    public static class SEOProcess
    {
        public static Result processText(SearchModel model, List<string> stopWords)
        {            
            Result viewResult = new Result();
            var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            viewResult.isURL = Uri.IsWellFormedUriString(model.textSearch, UriKind.Absolute);
          
           
                if (viewResult.isURL)
                {
                    var uri = new Uri(model.textSearch);
                    var baseUrl = uri.Scheme + "://" + uri.Host;

                    using (WebClient client = new WebClient())
                    {
                        string s = client.DownloadString(model.textSearch);
                        //Get repeatitive word
                        viewResult.stopWordInContent = model.isRepeat?getRepeatedWords(s, stopWords):new List<RepeatedWords>();
                        viewResult.stopWordInMeta = model.isRepeatMeta?getRepeatedWords(extractMetaTag(s), stopWords):new List<RepeatedWords>();
                        //Get External link                   
                        var baseLink = new Regex(baseUrl, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        viewResult.numberExternalLinks = model.isExternalLink?linkParser.Matches(s).Count - baseLink.Matches(s).Count :0;

                    }
                }
                else
                {
                    viewResult.filteredText = filterText(model.textSearch, stopWords);
                    viewResult.stopWordInContent = model.isRepeat?getRepeatedWords(model.textSearch, stopWords):new List<RepeatedWords>();
                    viewResult.numberExternalLinks = model.isExternalLink ? linkParser.Matches(model.textSearch).Count : 0;
                }
            
            return viewResult;
        }

        public static List<string> LoadJson()
        {
            using (var client = new WebClient())
            {
                string path = ConfigurationManager.AppSettings["SWPath"].ToString();
                List<string> items = JsonConvert.DeserializeObject<List<string>>(client.DownloadString(path));
                return items;
            }
        }

        private static List<RepeatedWords> getRepeatedWords(string source, List<string> stopwords)
        {

            List<RepeatedWords> repeatedList = new List<RepeatedWords>();
            foreach (var word in stopwords)
            {
                string pattern = @"(^|\s)" + word.ToLower() + "(\\s|$)";
                if (Regex.Matches(source.ToLower(), pattern).Count > 0)
                {
                    repeatedList.Add(new RepeatedWords
                    {
                        stopWord = word,
                        noOfTimes = Regex.Matches(source.ToLower(), pattern).Count
                    });
                }
            }
            return repeatedList;
        }

        private static string extractMetaTag(string source)
        {
            var tags = Regex.Matches(source, @"(?<tag>\<meta[^\>]*>)", RegexOptions.IgnoreCase);
            string metaTags = string.Empty;
            if (tags != null)
            {
                foreach (var meta in tags)
                {
                    metaTags += meta.ToString() + Environment.NewLine;

                }
            }
            return metaTags;
        }

        private static string filterText(string source, List<string> stopwords)
        {
            return source = Regex.Replace(source.ToLower(), "\\b" + string.Join("\\b|\\b", stopwords) + "\\b", "");
        }
    }
}