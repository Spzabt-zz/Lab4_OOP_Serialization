using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Serialization
{
    public class Concert : IXmlSerializable
    {
        private string _organizationName;
        private DateTime _concertDate;
        private List<Performance> _performances;

        public string OrganizationName
        {
            get => _organizationName;
            set => _organizationName = value;
        }

        public DateTime ConcertDate
        {
            get => _concertDate;
            set => _concertDate = value;
        }

        public List<Performance> Performances
        {
            get => _performances;
            set => _performances = value;
        }

        public Concert(string organizationName, DateTime concertDate, List<Performance> performances)
        {
            _organizationName = organizationName;
            _concertDate = concertDate;
            _performances = performances;
        }

        public Concert()
        {
            _performances = new List<Performance>();
        }

        public void AddPerformance()
        {
            throw new NotImplementedException();
        }

        public string ToShortString()
        {
            return $"Organization: {_organizationName}, Date: {_concertDate}";
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "OrganizationName":
                            reader.Read();
                            _organizationName = reader.Value;
                            break;
                        case "ConcertDate":
                            reader.Read();
                            _concertDate = DateTime.Parse(reader.Value);
                            break;
                        case "Performances":
                            _performances = Performance.ReadPerformances(reader);
                            //AddPerformance();
                            break;
                    }
                }

                if (reader.Name.Equals("Concert")) break;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Concert");
            writer.WriteElementString("OrganizationName", _organizationName);
            writer.WriteElementString("ConcertDate", _concertDate.ToString());
            writer.WriteStartElement("Performances");
            Performances?.ForEach(performance => { performance.WriteXml(writer); });
            writer.WriteEndElement();
        }

        public static List<Concert> ReadConcerts(string fileName)
        {
            var concerts = new List<Concert>();
            if (!File.Exists(fileName)) return concerts;
            using (var reader = XmlReader.Create(fileName))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    var concert = new Concert();
                    if (reader.IsStartElement() && !reader.Name.Equals("Concert"))
                    {
                        concert.ReadXml(reader);
                        concerts.Add(concert);
                    }
                    else break;
                }
            }

            return concerts;
        }

        public static void WriteConcert(string fileName, List<Concert> concerts)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                NewLineOnAttributes = true,
                ConformanceLevel = ConformanceLevel.Auto
            };

            var xmlWriter = XmlWriter.Create(fileName, settings);
            xmlWriter.WriteStartElement("Concerts");
            concerts.ForEach(journal => { journal.WriteXml(xmlWriter); });
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }

        public override string ToString()
        {
            foreach (var performance in _performances)
            {
                return
                    $"Organization: {_organizationName}, " +
                    $"Date: {_concertDate}, " +
                    $"Composition: {performance.Composition}, " +
                    $"Author name: {performance.Performer.Name}, " +
                    $"Author surname: {performance.Performer.Surname}, " +
                    $"Performance duration: {performance.PerformanceDuration}, " +
                    $"Performance name: {performance.NameOfComposition}";
            }

            return $"Organization: {_organizationName}, Date: {_concertDate}";
        }
    }
}