using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class MoreBuyingChoice
{
    [JsonProperty("price")]
    public TotalPrice Price { get; set; }

    [JsonProperty("seller_name")]
    public string SellerName { get; set; }

    [JsonProperty("free_shipping")]
    public bool FreeShipping { get; set; }

    [JsonProperty("position")]
    public long Position { get; set; }
}