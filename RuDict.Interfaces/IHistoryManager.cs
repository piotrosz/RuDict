using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RuDict.Model;

namespace RuDict.Interfaces
{
    public interface IHistoryManager
    {
        void Add(HistoryEntry entry);
        void Add(string entry);
        void Remove(string entry);
        void RemoveMany(List<string> entries);
        List<HistoryEntry> GetAll();
        void RemoveAll();
    }
}
