namespace ProductShop.Dtos.User
{
    using System.Linq;
    using Newtonsoft.Json;

    [JsonObject]
    public class ExportAllUsersDto
    {
        [JsonProperty("usersCount")]
        public int Count => this.Users.Any() ? this.Users.Length : 0;

        [JsonProperty("users")]
        public ExportUserWithSoldProductsDto[] Users { get; set; }
    }
}
