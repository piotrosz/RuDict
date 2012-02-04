using System;
using System.Collections.Generic;
using RuDict.Model;

namespace RuDict.Interfaces
{
    public interface IScrapper
    {
        List<SearchResult> Scrape(string htmlContents);
    }
}
