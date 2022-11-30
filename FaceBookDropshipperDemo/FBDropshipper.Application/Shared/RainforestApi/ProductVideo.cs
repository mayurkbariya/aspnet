using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class ProductVideo
{
    [JsonProperty("duration_seconds")]
    public long DurationSeconds { get; set; }

    [JsonProperty("width")]
    public long Width { get; set; }

    [JsonProperty("height")]
    public long Height { get; set; }

    [JsonProperty("link")]
    public string Link { get; set; }

    [JsonProperty("thumbnail")]
    public string Thumbnail { get; set; }

    [JsonProperty("is_hero_video")]
    public bool IsHeroVideo { get; set; }

    [JsonProperty("variant")]
    public string Variant { get; set; }

    [JsonProperty("group_id")]
    public string GroupId { get; set; }

    [JsonProperty("group_type")]
    public string GroupType { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }
}