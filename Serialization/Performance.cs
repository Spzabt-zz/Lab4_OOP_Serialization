using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Serialization
{
    public class Performance : IXmlSerializable
    {
        private Performer _performer;
        private Composition _composition;
        private int _performanceDuration;
        private string _nameOfComposition;

        public Performer Performer
        {
            get => _performer;
            set => _performer = value;
        }

        public Composition Composition
        {
            get => _composition;
            set => _composition = value;
        }

        public int PerformanceDuration
        {
            get => _performanceDuration;
            set => _performanceDuration = value;
        }

        public string NameOfComposition
        {
            get => _nameOfComposition;
            set => _nameOfComposition = value;
        }

        public Performance(Performer performer, Composition composition, int performanceDuration,
            string nameOfComposition)
        {
            _performer = performer;
            _composition = composition;
            _performanceDuration = performanceDuration;
            _nameOfComposition = nameOfComposition;
        }

        public Performance()
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
                        case "Performer":
                            _performer = new Performer();
                            _performer.ReadXml(reader);
                            break;
                        case "Composition":
                            reader.Read();
                            _composition = (Composition) Enum.Parse(typeof(Composition), reader.Value);
                            break;
                        case "PerformanceDuration":
                            reader.Read();
                            _performanceDuration = int.Parse(reader.Value);
                            break;
                        case "NameOfComposition":
                            reader.Read();
                            _nameOfComposition = reader.Value;
                            break;
                    }
                }

                if (reader.Name.Equals("Performance")) break;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Performance");
            _performer.WriteXml(writer);
            writer.WriteElementString("Composition", _composition.ToString());
            writer.WriteElementString("PerformanceDuration", _performanceDuration.ToString());
            writer.WriteElementString("NameOfComposition", _nameOfComposition);
            writer.WriteEndElement();
        }

        public static List<Performance> ReadPerformances(XmlReader reader)
        {
            var performances = new List<Performance>();
            reader.MoveToContent();
            var performance = new Performance();
            while (reader.Read())
            {
                if (reader.IsStartElement() && !reader.Name.Equals("Performances"))
                {
                    performance.ReadXml(reader);
                    performances.Add(performance);
                }
                else break;
            }

            return performances;
        }

        public void WritePerformance(string fileName, List<Performance> performances)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                NewLineOnAttributes = true,
                ConformanceLevel = ConformanceLevel.Auto
            };

            var xmlWriter = XmlWriter.Create(fileName, settings);
            xmlWriter.WriteStartElement("Performances");
            performances.ForEach(performance => { performance.WriteXml(xmlWriter); });
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }

        public override string ToString()
        {
            return $"Composition name {_nameOfComposition}, duration {_performanceDuration}";
        }
    }
}