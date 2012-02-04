using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace RuDict.Interfaces
{
    public interface IDownloader
    {
        void DownloadAsync(
            DownloadProgressChangedEventHandler downloadProgressChangedHandler,
            DownloadStringCompletedEventHandler downloadCompetedHandler, 
            string word);
    }
}
