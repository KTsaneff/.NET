namespace ProductShop.Dtos.Product
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ExportSimpleProductDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
