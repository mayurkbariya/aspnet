using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class Condition
{
    [JsonProperty("is_new")]
    public bool IsNew { get; set; }
}