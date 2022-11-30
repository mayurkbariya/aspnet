using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class ProductElement
{
    [JsonProperty("asin")]
    public string Asin { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("link")]
    public string Link { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("price")]
    public TotalPrice Price { get; set; }
}