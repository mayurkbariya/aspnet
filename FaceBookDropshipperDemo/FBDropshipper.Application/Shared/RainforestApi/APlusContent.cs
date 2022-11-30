using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class APlusContent
{
    [JsonProperty("has_a_plus_content")]
    public bool HasAPlusContent { get; set; }

    [JsonProperty("has_brand_story")]
    public bool HasBrandStory { get; set; }

    [JsonProperty("brand_story")]
    public BrandStory BrandStory { get; set; }

    [JsonProperty("third_party")]
    public bool ThirdParty { get; set; }
}