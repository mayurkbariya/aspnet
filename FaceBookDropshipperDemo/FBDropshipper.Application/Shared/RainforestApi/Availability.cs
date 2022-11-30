using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class Availability
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("raw")]
    public string Raw { get; set; }

    [JsonProperty("dispatch_days")]
    public long DispatchDays { get; set; }
}