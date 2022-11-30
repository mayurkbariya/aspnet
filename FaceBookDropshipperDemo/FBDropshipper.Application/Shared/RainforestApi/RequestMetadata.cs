using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class RequestMetadata
{
    [JsonProperty("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("processed_at")]
    public DateTimeOffset ProcessedAt { get; set; }

    [JsonProperty("total_time_taken")]
    public double TotalTimeTaken { get; set; }

    [JsonProperty("amazon_url")]
    public string AmazonUrl { get; set; }
}