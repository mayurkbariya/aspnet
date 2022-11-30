using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class Profile
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
    public string Link { get; set; }

    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }
}