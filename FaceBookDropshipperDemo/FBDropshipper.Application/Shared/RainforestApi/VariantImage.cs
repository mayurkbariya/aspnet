using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class VariantImage
{
    [JsonProperty("link")]
    public string Link { get; set; }

    [JsonProperty("variant")]
    public string Variant { get; set; }
}