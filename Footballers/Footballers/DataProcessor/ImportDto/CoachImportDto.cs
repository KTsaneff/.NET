using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class CoachImportDto
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [XmlElement("Nationality")]
        [Required(AllowEmptyStrings = false)]
        public  string Nationality { get; set; }

        [XmlArray("Footballers")]
        public FootballerImportDto[] Footballers { get; set; }
    }
}
