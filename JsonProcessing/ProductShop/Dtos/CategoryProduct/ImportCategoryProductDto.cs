using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Dtos.CategoryProduct
{
    [JsonObject]
    public class ImportCategoryProductDto
    {
        [JsonProperty(nameof(CategoryId))]
        public int CategoryId { get; set; }


        [JsonProperty(nameof(ProductId))]
        public int ProductId { get; set; }
    }
}
