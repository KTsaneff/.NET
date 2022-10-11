using Newtonsoft.Json;
using SoftJail.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentWithCellsDto
    {
        [Required]
        [MinLength(ValidationConstants.DepartmentNameMinLength)]
        [MaxLength(ValidationConstants.DepartmentNameMaxLength)]
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(nameof(Cells))]
        public ImportDepartmentCellDto[] Cells { get; set; }
    }
}
