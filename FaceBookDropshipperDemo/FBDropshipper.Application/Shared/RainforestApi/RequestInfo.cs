using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class RequestInfo
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("credits_used")]
    public long CreditsUsed { get; set; }

    [JsonProperty("credits_remaining")]
    public long CreditsRemaining { get; set; }

    [JsonProperty("credits_used_this_request")]
    public long CreditsUsedThisRequest { get; set; }
}