using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RuDict.Interfaces;
using RuDict.Model;
using Raven.Client;
using Raven.Client.Embedded;

namespace RuDict.History
{
    public class RavenHistoryManager : IHistoryManager
    {
        private static readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\db";

        private static IDocumentStore documentStore = null;

        protected IDocumentStore DocumentStore
        {
            get
            {
                if (documentStore == null)
                {
                    documentStore = new EmbeddableDocumentStore
                    {
                        DataDirectory = path
                    };
                    documentStore.Initialize();
                }

                return documentStore;
            }
        }

        public void Add(HistoryEntry entry)
        {
            using (var session = DocumentStore.OpenSession())
            {
                session.Store(entry);
                session.SaveChanges();
            }
        }

        public void Add(string entry)
        {
            if (string.IsNullOrWhiteSpace(entry))
                return;

            using (var session = DocumentStore.OpenSession())
            {
                if (session.Query<HistoryEntry>().SingleOrDefault(x => x.Word == entry) == null)
                {
                    session.Store(new HistoryEntry { Date = DateTime.Now, Word = entry });
                    session.SaveChanges();
                }
            }
        }

        public void Remove(string entry)
        {
            using (var session = DocumentStore.OpenSession())
            {
                session.Delete<HistoryEntry>(session.Query<HistoryEntry>().SingleOrDefault(x => x.Word == entry));
                session.SaveChanges();
            }
        }

        public void RemoveMany(List<string> entries)
        {
            using (var session = DocumentStore.OpenSession())
            {
                foreach (var item in entries)
                {
                    var toDelete = session.Query<HistoryEntry>().FirstOrDefault(x => x.Word == item);
                    if(toDelete != null)
                    {
                        session.Delete<HistoryEntry>(toDelete);
                    }
                }

                session.SaveChanges();
            }
        }

        public List<HistoryEntry> GetAll()
        {
            var result = new List<HistoryEntry>();

            using (var session = DocumentStore.OpenSession())
            {
                result = session.Query<HistoryEntry>()
                    .Take(1000)
                    .OrderByDescending(x => x.Date)
                    .ToList();
            }

            return result;
        }

        public void RemoveAll()
        {
            using (var session = DocumentStore.OpenSession())
            {
                var list = session.Query<HistoryEntry>().ToList();

                for (int i = 0; i < list.Count; i++)
                {
                    session.Delete<HistoryEntry>(list[i]);
                }

                session.SaveChanges();
            }
        }
    }
}
