using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class BuyboxWinner
{
    [JsonProperty("offer_id")]
    public string OfferId { get; set; }

    [JsonProperty("new_offers_count")]
    public long NewOffersCount { get; set; }

    [JsonProperty("new_offers_from")]
    public TotalPrice NewOffersFrom { get; set; }

    [JsonProperty("is_prime")]
    public bool IsPrime { get; set; }

    [JsonProperty("is_amazon_fresh")]
    public bool IsAmazonFresh { get; set; }

    [JsonProperty("condition")]
    public Condition Condition { get; set; }

    [JsonProperty("availability")]
    public Availability Availability { get; set; }

    [JsonProperty("fulfillment")]
    public Fulfillment Fulfillment { get; set; }

    [JsonProperty("price")]
    public TotalPrice Price { get; set; }

    [JsonProperty("rrp")]
    public TotalPrice Rrp { get; set; }

    [JsonProperty("shipping")]
    public Shipping Shipping { get; set; }
}