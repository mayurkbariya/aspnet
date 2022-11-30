using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class Section
{
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; }
}