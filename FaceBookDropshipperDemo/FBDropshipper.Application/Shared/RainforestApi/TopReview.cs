using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class TopReview
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; }

    [JsonProperty("body_html")]
    public string BodyHtml { get; set; }

    [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
    public string Link { get; set; }

    [JsonProperty("rating")]
    public long Rating { get; set; }

    [JsonProperty("date")]
    public Date Date { get; set; }

    [JsonProperty("profile")]
    public Profile Profile { get; set; }

    [JsonProperty("vine_program")]
    public bool VineProgram { get; set; }

    [JsonProperty("verified_purchase")]
    public bool VerifiedPurchase { get; set; }

    [JsonProperty("videos", NullValueHandling = NullValueHandling.Ignore)]
    public List<TopReviewVideo> Videos { get; set; }

    [JsonProperty("helpful_votes", NullValueHandling = NullValueHandling.Ignore)]
    public long? HelpfulVotes { get; set; }

    [JsonProperty("review_country")]
    public string ReviewCountry { get; set; }

    [JsonProperty("is_global_review")]
    public bool IsGlobalReview { get; set; }

    [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
    public List<MainImageElement> Images { get; set; }
}