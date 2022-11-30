using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class ImportantInformation
{
    [JsonProperty("sections")]
    public List<Section> Sections { get; set; }
}