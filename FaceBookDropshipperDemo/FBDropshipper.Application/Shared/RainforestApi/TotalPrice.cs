using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class TotalPrice
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("value")]
    public double Value { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; }

    [JsonProperty("raw")]
    public string Raw { get; set; }
}