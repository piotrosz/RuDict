using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RuDict.Google
{
    [Serializable]
    [DataContract]
    internal class Result
    {
        [DataMember(Name = "query")]
        public string Query { get; set; }

        [DataMember(Name = "sourceLanguage")]
        public string SourceLanguage { get; set; }

        [DataMember(Name = "targetLanguage")]
        public string TargetLanguage { get; set; }

        [DataMember(Name = "webDefinitions")]
        public List<WebDefinition> WebDefinitions { get; set; }
    }

    [DataContract]
    internal class WebDefinition
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "terms")]
        public List<Term> Terms { get; set; }

        [DataMember(Name = "entries")]
        public List<Entry> Entries { get; set; }
    }

    [DataContract]
    internal class Term
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "language")]
        public string Language { get; set; }
    }

    [DataContract]
    internal class Entry
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "terms")]
        public List<Term> Terms { get; set; }
    }
}
