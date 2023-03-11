using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Theatre.DataProcessor.ImportDto
{
    public class TicketImportDto
    {
        [Required]
        [Range(typeof(decimal), "1.00", "100.00", ConvertValueInInvariantCulture = true, ParseLimitsInInvariantCulture = true)]
        [JsonProperty("Price")]
        public decimal Price { get; set; }

        [Required]
        [Range(typeof(sbyte), "1", "10")]
        [JsonProperty("RowNumber")]
        public sbyte RowNumber { get; set; }

        [Required]
        [JsonProperty("PlayId")]
        public int PlayId { get; set; }
    }
}
