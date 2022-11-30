using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class TopReviewVideo
{
    [JsonProperty("video")]
    public string Video { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }
}