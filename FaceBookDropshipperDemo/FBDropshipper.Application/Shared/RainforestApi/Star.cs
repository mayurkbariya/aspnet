using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class Star
{
    [JsonProperty("percentage")]
    public long Percentage { get; set; }

    [JsonProperty("count")]
    public long Count { get; set; }
}