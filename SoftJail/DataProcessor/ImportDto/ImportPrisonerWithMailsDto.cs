using Newtonsoft.Json;
using SoftJail.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonerWithMailsDto
    {
        [Required]
        [MinLength(ValidationConstants.PrisonerFullNameMinLength)]
        [MaxLength(ValidationConstants.PrisonerFullNameMaxLength)]
        [JsonProperty(nameof(FullName))]
        public string FullName { get; set; }


        [Required]
        [RegularExpression(ValidationConstants.PrisonerNicknameRegex)]
        [JsonProperty(nameof(Nickname))]
        public string Nickname { get; set; }

        [Required]
        [Range(ValidationConstants.PrisonerAgeMinValue, ValidationConstants.PrisonerAgeMaxValue)]
        [JsonProperty(nameof(Age))]
        public int Age { get; set; }

        [Required]
        [JsonProperty(nameof(IncarcerationDate))]
        public string IncarcerationDate { get; set; }

        [JsonProperty(nameof(ReleaseDate))]
        public string ReleaseDate { get; set; }

        [Range(typeof(decimal), ValidationConstants.NonNegativeDecimalMinValue, ValidationConstants.NonNegativeDecimalMaxValue)]
        [JsonProperty(nameof(Bail))]
        public decimal? Bail { get; set; }

        [JsonProperty(nameof(CellId))]
        public int? CellId { get; set; }

        [JsonProperty(nameof(Mails))]
        public ImportPrisonerMailDto[] Mails { get; set; }
    }
}
