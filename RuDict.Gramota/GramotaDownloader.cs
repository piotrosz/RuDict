using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using RuDict.Interfaces;

namespace RuDict.Gramota
{
    public class GramotaDownloader : IDownloader
    {
        public void DownloadAsync(
            DownloadProgressChangedEventHandler downloadProgressChangedHandler, 
            DownloadStringCompletedEventHandler downloadCompetedHandler, string word)
        {
            string url = string.Format("http://gramota.ru/slovari/dic/?word={0}&all=x", HttpUtility.UrlEncode(word, Encoding.GetEncoding("windows-1251")));

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.GetEncoding("windows-1251");

                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloadProgressChangedHandler);
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(downloadCompetedHandler);
                client.DownloadStringAsync(new Uri(url));
            }
        }
    }
}
