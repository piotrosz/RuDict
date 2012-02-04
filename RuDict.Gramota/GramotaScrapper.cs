using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using RuDict.Interfaces;
using RuDict.Model;

namespace RuDict.Gramota
{
    public class GramotaScrapper : IScrapper
    {
        public List<SearchResult> Scrape(string contents)
        {
            var result = new List<SearchResult>();

            Regex r = new Regex(@"<h2>(.+?)</h2>");
            Match match;
            for (match = r.Match(contents); match.Success; match = match.NextMatch())
            {
                result.Add(new SearchResult { Source = match.Groups[1].ToString() });
            }

            r = new Regex(@"<div style=""padding-left:50px"">(.+?)</div>");

            int i = 0;
            for (match = r.Match(contents); match.Success; match = match.NextMatch())
            {
                result[i++].Description = match.Groups[1].ToString();
            }

            return result;
        }
    }
}
