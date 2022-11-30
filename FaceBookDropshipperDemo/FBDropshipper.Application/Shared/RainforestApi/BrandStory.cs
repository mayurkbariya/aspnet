using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class BrandStory
{
    [JsonProperty("hero_image")]
    public string HeroImage { get; set; }

    [JsonProperty("images")]
    public List<string> Images { get; set; }
}