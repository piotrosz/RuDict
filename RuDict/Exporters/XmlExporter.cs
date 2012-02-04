using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RuDict.Model;
using RuDict.History;

namespace RuDict.Exporters
{
    public class XmlExporter
    {
        public void Export(List<HistoryEntry> list, string path)
        {
            XmlHistoryManager m = new XmlHistoryManager();
            m.Serialize(list, path);
        }
    }
}
