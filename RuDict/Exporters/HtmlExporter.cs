using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RuDict.Model;
using System.IO;

namespace RuDict.Exporters
{
    public class HtmlExporter
    {
        public void Export(List<HistoryEntry> list, string path)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendFormat("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            sb.AppendLine();
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");

            sb.AppendLine("<table>");
            sb.AppendFormat("<tr><th>Słowo</th><th>Data</th></tr>");
            sb.AppendLine();
            foreach (var item in list)
            {
                sb.AppendFormat("<tr><td>{0}</td><td>{1:yyyy-MM-dd}</td></tr>", item.Word, item.Date);
                sb.AppendLine();
            }
            sb.AppendLine("</table>");

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            File.WriteAllText(path, sb.ToString());
        }
    }
}
