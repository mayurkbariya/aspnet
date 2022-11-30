using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class BestsellersRank
{
    [JsonProperty("category")]
    public string Category { get; set; }

    [JsonProperty("rank")]
    public long Rank { get; set; }

    [JsonProperty("link")]
    public string Link { get; set; }
}