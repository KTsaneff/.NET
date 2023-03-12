using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class CoachExportDto
    {
        [XmlElement("CoachName")]
        public string CoachName { get; set; }

        [XmlArray("Footballers")]
        public FootballerExportDto[] Footballers { get; set; }

        [XmlAttribute("FootballersCount")]
        public string FootballersCount { get; set; }
    }
}
