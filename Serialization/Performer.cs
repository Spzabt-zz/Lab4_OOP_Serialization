using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Serialization
{
    public class Performer : IXmlSerializable
    {
        private string _name;
        private string _surname;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Surname
        {
            get => _surname;
            set => _surname = value;
        }

        public Performer(string name, string surname)
        {
            _name = name;
            _surname = surname;
        }

        public Performer()
        {
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "Name":
                            reader.Read();
                            _name = reader.Value;
                            break;
                        case "Surname":
                            reader.Read();
                            _surname = reader.Value;
                            break;
                    }
                }

                if (reader.Name.Equals("Performer")) break;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Performer");
            writer.WriteElementString("Name", _name);
            writer.WriteElementString("Surname", _surname);
            writer.WriteEndElement();
        }

        public static List<Performer> ReadPerformers(string filename)
        {
            var performers = new List<Performer>();

            if (!File.Exists(filename)) return performers;
            using (var reader = XmlReader.Create(filename))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.IsStartElement() && !reader.Name.Equals("Performers"))
                    {
                        var performer = new Performer();
                        performer.ReadXml(reader);
                        performers.Add(performer);
                    }
                    else break;
                }
            }

            return performers;
        }

        public static void WritePerformer(string filename, List<Performer> performers)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                NewLineOnAttributes = true,
                ConformanceLevel = ConformanceLevel.Auto
            };

            var xmlWriter = XmlWriter.Create(filename, settings);
            xmlWriter.WriteStartElement("Performers");
            performers.ForEach(wood =>
            {
                wood.WriteXml(xmlWriter);
            });
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }

        public override string ToString()
        {
            return $"{Name} {Surname}";
        }
    }
}