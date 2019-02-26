using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ConsoleApp
{
    [XmlRoot("words")]
    public class WordSettings<TKey, TValue> : Dictionary<TKey, TValue>,
                                               IXmlSerializable
    {
        public XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement) { return; }

            reader.Read();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                object key = reader.GetAttribute("text");
                object value = reader.GetAttribute("count");
                this.Add((TKey)key, (TValue)value);
                reader.Read();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var dict in this)
            {
                writer.WriteStartElement("word");
                writer.WriteAttributeString("text", dict.Key.ToString());
                writer.WriteAttributeString("count", dict.Value.ToString());
                writer.WriteEndElement();
            }
        }
    }
}
