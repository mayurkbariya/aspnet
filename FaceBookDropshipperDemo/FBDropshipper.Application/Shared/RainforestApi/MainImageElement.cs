using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class MainImageElement
{
    [JsonProperty("link")]
    public string Link { get; set; }
}