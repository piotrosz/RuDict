using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RuDict.Model;
using System.IO;

namespace RuDict.Exporters
{
    public class CsvExporter
    {
        private const string separator = ";";
        public void Export(List<HistoryEntry> list, string path)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.AppendFormat("{1}{0}{2:yyyy-MM-dd}{0}", separator, item.Word, item.Date);
                sb.AppendLine();
            }

            File.WriteAllText(path, sb.ToString());
        }
    }
}
