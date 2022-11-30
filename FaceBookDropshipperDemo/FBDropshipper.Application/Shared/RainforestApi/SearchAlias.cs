using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class SearchAlias
{
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }
}