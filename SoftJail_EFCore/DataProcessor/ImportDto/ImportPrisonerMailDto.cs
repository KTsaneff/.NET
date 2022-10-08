using Newtonsoft.Json;
using SoftJail.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonerMailDto
    {
        [Required]
        [JsonProperty(nameof(Description))]
        public string Description { get; set; }

        [Required]
        [JsonProperty(nameof(Sender))]
        public string Sender { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.MailAddressRegex)]
        [JsonProperty(nameof(Address))]
        public string Address { get; set; }
    }
}
