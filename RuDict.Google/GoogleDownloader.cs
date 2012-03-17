using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

using RuDict.Interfaces;

namespace RuDict.Google
{
    public class GoogleDownloader : IDownloader
    {
        public void DownloadAsync(
            DownloadStringCompletedEventHandler downloadCompetedHandler, 
            string word)
        {
            string url = string.Format("http://www.google.com/dictionary/json?callback=dict_api.callbacks.id100&q={0}&sl=ru&tl=ru&restrict=pr%2Cde&client=te", HttpUtility.UrlEncode(word));

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;

                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(downloadCompetedHandler);
                client.DownloadStringAsync(new Uri(url));
            }
        }
    }
}
