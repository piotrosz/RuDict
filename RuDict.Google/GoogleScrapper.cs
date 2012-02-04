using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json; // reference: System.ServiceModel.Web
using RuDict.Interfaces;
using RuDict.Model;

namespace RuDict.Google
{
    public class GoogleScrapper : IScrapper
    {
        public List<SearchResult> Scrape(string contents)
        {
            string jsonText = contents
                .Replace(@"\x3c", "<")
                .Replace(@"\x3e", ">")
                .Replace(@"\x3d", "=")
                .Replace(@"\x22", "'")
                .Replace(@"\x26", "&")
                .Replace(@"\xad", "")
                .Replace("dict_api.callbacks.id100(", "")
                .Replace(",200,null)", "");

            List<SearchResult> search = new List<SearchResult>();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Result));

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonText)))
            {
                Result result = (Result)serializer.ReadObject(stream);

                if (result.WebDefinitions != null)
                {
                    foreach (var item in result.WebDefinitions)
                    {
                        foreach (var subitem in item.Entries)
                        {
                            var text = subitem.Terms.Where(x => x.Type == "text").Select(x => x.Text).SingleOrDefault();
                            var source = subitem.Terms.Where(x => x.Type == "url").Select(x => x.Text).SingleOrDefault();

                            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(source))
                            {
                                search.Add(new SearchResult
                                {
                                    Source = source,
                                    Description = text
                                });
                            }
                        }
                    }
                }
            }

            return search;
        }
    }
}
