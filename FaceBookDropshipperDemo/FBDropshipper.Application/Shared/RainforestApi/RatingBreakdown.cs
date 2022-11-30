using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class RatingBreakdown
{
    [JsonProperty("five_star")]
    public Star FiveStar { get; set; }

    [JsonProperty("four_star")]
    public Star FourStar { get; set; }

    [JsonProperty("three_star")]
    public Star ThreeStar { get; set; }

    [JsonProperty("two_star")]
    public Star TwoStar { get; set; }

    [JsonProperty("one_star")]
    public Star OneStar { get; set; }
}