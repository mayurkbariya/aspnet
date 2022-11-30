using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class StandardDelivery
{
    [JsonProperty("date")]
    public string Date { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}