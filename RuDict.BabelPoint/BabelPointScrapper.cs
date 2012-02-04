using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RuDict.Interfaces;
using RuDict.Model;

namespace RuDict.BabelPoint
{
    public class BabelPointScrapper : IScrapper
    {
        public List<SearchResult> Scrape(string htmlContents)
        {
            var result = new List<SearchResult>();

            Regex regex = new Regex(@"(<table class=""tbrei6"" align=center>[^\}]+</table>)");
            Match match;
            for (match = regex.Match(htmlContents); match.Success; match = match.NextMatch())
            {
                result.Add(new SearchResult 
                { 
                    Description = match.Groups[1].ToString().Replace("align=center", "")
                });
            }
            return result;
        }
    }
}
