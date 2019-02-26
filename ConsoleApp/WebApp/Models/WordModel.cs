using System;
using System.Xml.Serialization;

namespace WebApp.Models
{
    [Serializable]
    [XmlRoot("words"), XmlType("words")]
    public class WordModel
    {
        public string word { get; set; }
        public int count { get; set; }

    }
}