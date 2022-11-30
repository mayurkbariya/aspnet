using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class AddAnAccessory
{
    [JsonProperty("asin")]
    public string Asin { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("price")]
    public TotalPrice Price { get; set; }
}