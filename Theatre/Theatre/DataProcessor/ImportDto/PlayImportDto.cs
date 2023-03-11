using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class PlayImportDto
    {
        [Required]
        [StringLength(50, MinimumLength = 4)]
        [XmlElement("Title")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        [XmlElement("Duration")]
        public string Duration { get; set; }

        [Range(typeof(float), "0.00", "10.00", ConvertValueInInvariantCulture = true, ParseLimitsInInvariantCulture = true)]
        [XmlElement("Rating")]
        public float Rating { get; set; }

        [Required(AllowEmptyStrings = false)]
        [XmlElement("Genre")]
        public string Genre { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(700)]
        [XmlElement("Description")]
        public string Description { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        [XmlElement("Screenwriter")]
        public string Screenwriter { get; set; }
    }
}
