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
        public void DownloadAsync(
            DownloadProgressChangedEventHandler downloadProgressChangedHandler, 
            DownloadStringCompletedEventHandler downloadCompetedHandler, 
            string word)
        {
            string url = string.Format("http://www.babelpoint.org/russian/glance.php?q={0}", HttpUtility.UrlEncode(word, Encoding.UTF8));

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloadProgressChangedHandler);
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(downloadCompetedHandler);
                client.DownloadStringAsync(new Uri(url));
            }
        }
    }
}
