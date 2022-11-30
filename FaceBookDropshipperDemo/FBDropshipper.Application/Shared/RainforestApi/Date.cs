using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class Date
{
    [JsonProperty("raw")]
    public string Raw { get; set; }

    [JsonProperty("utc")]
    public DateTimeOffset Utc { get; set; }
}