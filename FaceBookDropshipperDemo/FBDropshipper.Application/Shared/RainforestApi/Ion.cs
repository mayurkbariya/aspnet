using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class Ion
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }
}