using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class Fulfillment
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("standard_delivery")]
    public StandardDelivery StandardDelivery { get; set; }

    [JsonProperty("is_sold_by_amazon")]
    public bool IsSoldByAmazon { get; set; }

    [JsonProperty("is_fulfilled_by_amazon")]
    public bool IsFulfilledByAmazon { get; set; }

    [JsonProperty("is_fulfilled_by_third_party")]
    public bool IsFulfilledByThirdParty { get; set; }

    [JsonProperty("is_sold_by_third_party")]
    public bool IsSoldByThirdParty { get; set; }
}