using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class FrequentlyBoughtTogether
{
    [JsonProperty("total_price")]
    public TotalPrice TotalPrice { get; set; }

    [JsonProperty("products")]
    public List<ProductElement> Products { get; set; }
}