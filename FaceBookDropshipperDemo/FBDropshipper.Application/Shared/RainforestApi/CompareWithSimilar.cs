using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class CompareWithSimilar
{
    [JsonProperty("asin")]
    public string Asin { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("rating")]
    public double Rating { get; set; }

    [JsonProperty("ratings_total")]
    public long RatingsTotal { get; set; }

    [JsonProperty("price")]
    public TotalPrice Price { get; set; }

    [JsonProperty("link")]
    public string Link { get; set; }

    [JsonProperty("is_prime", NullValueHandling = NullValueHandling.Ignore)]
    public bool? IsPrime { get; set; }
}