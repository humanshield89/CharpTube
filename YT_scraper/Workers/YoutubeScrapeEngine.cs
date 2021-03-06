﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YT_scraper.Data;

namespace YT_scraper.Workers
{
    class YoutubeScrapeEngine
    {
        static Scrape scraper = new Scrape();
        public static List<VideoItem> scrapeYoutube(String searchQuery)
        {

            List<VideoItem> results = new List<VideoItem>();
            using (WebClient webClient = new WebClient())
            {
                string url;
                if (!searchQuery.Contains(' ') && searchQuery.Length != 11)
                {
                    url = urlBuilder(searchQuery);
                }
                else
                {
                    url = "https://www.youtube.com/results?search_query="+searchQuery;
                }
                 
                string data;
                try
                {
                    data = webClient.DownloadString(url);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //webClient.DownloadFile(url , "result");
                //string data = File.ReadAllText("result");

                ScrapeCriteria scrapeCriteria = new ScrapeCriteria(data , Constants.pattern );
                MatchCollection matches = scraper.scrape(scrapeCriteria);
                foreach (Match element in matches)
                {
                    Match match = Regex.Match(element.Groups[0].Value, Constants.pattern, RegexOptions.Singleline);
                    if (match.Success )
                    {
                        if (match.Groups[1].Value.Length == 11)
                        results.Add(new VideoItem(match.Groups[2].Value, match.Groups[1].Value));
                    }
                }
            }
            return results;
        }

        public static string urlBuilder(string searchQuery)
        {
            
            return "https://www.youtube.com/results?search_query=" +WebUtility.UrlEncode(searchQuery);
        }
    }
}
