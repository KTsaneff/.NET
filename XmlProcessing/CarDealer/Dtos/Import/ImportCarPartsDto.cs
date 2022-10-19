using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("partId")]
    public class ImportCarPartsDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
