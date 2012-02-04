using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using RuDict.Model;
using RuDict.Interfaces;

namespace RuDict.History
{
    public class XmlHistoryManager : IHistoryManager
    {
        public XmlHistoryManager()
        {
            if (!File.Exists(path))
            {
                Serialize(new List<HistoryEntry>());
            }
        }

        private static readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\history.xml";

        private void Serialize(List<HistoryEntry> items)
        {
            var doc = new XmlDocument();

            var serializer = new XmlSerializer(typeof(List<HistoryEntry>));
            var stream = new MemoryStream();
            try
            {
                serializer.Serialize(stream, items);
                stream.Position = 0;
                doc.Load(stream);
                doc.Save(path);
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
        }

        public void Serialize(List<HistoryEntry> items, string filename)
        {
            var doc = new XmlDocument();

            var serializer = new XmlSerializer(typeof(List<HistoryEntry>));
            var stream = new MemoryStream();
            try
            {
                serializer.Serialize(stream, items);
                stream.Position = 0;
                doc.Load(stream);
                doc.Save(filename);
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
        }

        private List<HistoryEntry> Deserialize()
        {
            var result = new List<HistoryEntry>();
            var serializer = new XmlSerializer(typeof(List<HistoryEntry>));

            using (var reader = File.OpenRead(path))
            {
                using (var xReader = XmlReader.Create(reader))
                {
                    result = (List<HistoryEntry>)serializer.Deserialize(xReader);
                }
            }

            return result;
        }

        public void Add(HistoryEntry item)
        {
            item.Date = DateTime.Now;
            var list = Deserialize();
            list.Add(item);
            Serialize(list);
        }

        public void Add(string item)
        {
            Add(new HistoryEntry { Word = item });
        }

        public void Remove(string item)
        {
            var list = Deserialize();
            list = list.Where(x => x.Word != item).ToList();
            Serialize(list);
        }

        public void RemoveMany(List<string> items)
        {
            var list = Deserialize();
            list = list.Where(x => !items.Contains(x.Word)).ToList();
            Serialize(list);
        }

        public List<HistoryEntry> GetAll()
        {
            return Deserialize().OrderByDescending(x => x.Date).ToList();
        }

        public void RemoveAll()
        {
            Serialize(new List<HistoryEntry>());
        }
    }
}
