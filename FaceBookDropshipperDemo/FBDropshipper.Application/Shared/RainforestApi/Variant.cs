using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class Variant
{
    [JsonProperty("asin")]
    public string Asin { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("is_current_product")]
    public bool IsCurrentProduct { get; set; }

    [JsonProperty("link")]
    public string Link { get; set; }

    [JsonProperty("dimensions")]
    public List<Ion> Dimensions { get; set; }

    [JsonProperty("main_image")]
    public string MainImage { get; set; }

    [JsonProperty("images")]
    public List<VariantImage> Images { get; set; }

    [JsonProperty("price")]
    public TotalPrice Price { get; set; }
}