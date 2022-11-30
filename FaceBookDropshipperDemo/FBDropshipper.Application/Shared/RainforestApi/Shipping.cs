using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class Shipping
{
    [JsonProperty("raw")]
    public string Raw { get; set; }
}