using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class Category
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
    public string Link { get; set; }

    [JsonProperty("category_id", NullValueHandling = NullValueHandling.Ignore)]
    public long? CategoryId { get; set; }
}