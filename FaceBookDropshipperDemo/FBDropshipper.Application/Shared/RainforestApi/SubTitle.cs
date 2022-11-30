using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class SubTitle
{
    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("link")]
    public string Link { get; set; }
}