using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class RequestParameters
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("asin")]
    public string Asin { get; set; }

    [JsonProperty("amazon_domain")]
    public string AmazonDomain { get; set; }
}