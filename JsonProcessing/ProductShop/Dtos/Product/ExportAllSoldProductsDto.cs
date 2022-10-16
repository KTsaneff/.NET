namespace ProductShop.Dtos.Product
{
    using System.Linq;
    using Newtonsoft.Json;

    [JsonObject]
    public class ExportAllSoldProductsDto
    {
        [JsonProperty("count")]
        public int Count => Products.Any() ? Products.Length : 0;

        [JsonProperty("products")]
        public ExportSimpleProductDto[] Products { get; set; }
    }
}
