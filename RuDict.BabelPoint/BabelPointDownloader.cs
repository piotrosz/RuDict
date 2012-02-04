using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using RuDict.Interfaces;
using RuDict.Utils;
using System.Net.Cache;

namespace RuDict.BabelPoint
{
    public class BabelPointDownloader : IDownloader
    {
        CookieContainer cookies = new CookieContainer();

        public void DownloadAsync(
            DownloadProgressChangedEventHandler downloadProgressChangedHandler, 
            DownloadStringCompletedEventHandler downloadCompetedHandler, 
            string word)
        {
            string url = string.Format("http://www.babelpoint.org/russian/glance.php?q={0}", HttpUtility.UrlEncode(word, Encoding.UTF8));

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                //client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                //client.Headers.Add("Accept-Encoding", "gzip, deflate");
                //client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:9.0.1) Gecko/20100101 Firefox/9.0.1");
                //client.Headers.Add("Accept-Charset", "ISO-8859-2,utf-8;q=0.7,*;q=0.7");
                //client.Headers.Add("Content-Type", "text/html; charset=UTF-8");
                //client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.Default);

                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloadProgressChangedHandler);
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(downloadCompetedHandler);
                client.DownloadStringAsync(new Uri(url));
            }
        }
    }
}
