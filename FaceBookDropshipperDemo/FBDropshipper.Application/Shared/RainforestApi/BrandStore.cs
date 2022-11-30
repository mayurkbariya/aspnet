using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class BrandStore
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("link")]
    public string Link { get; set; }
}