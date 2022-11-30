using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi
{
    public class RainforestProductResponse
    {
        [JsonProperty("request_info")]
        public RequestInfo RequestInfo { get; set; }

        [JsonProperty("request_metadata")]
        public RequestMetadata RequestMetadata { get; set; }

        [JsonProperty("request_parameters")]
        public RequestParameters RequestParameters { get; set; }

        [JsonProperty("product")]
        public RainforestProduct Product { get; set; }

        [JsonProperty("brand_store")]
        public BrandStore BrandStore { get; set; }

        [JsonProperty("frequently_bought_together")]
        public FrequentlyBoughtTogether FrequentlyBoughtTogether { get; set; }

        [JsonProperty("similar_to_consider")]
        public SimilarToConsider SimilarToConsider { get; set; }

        [JsonProperty("compare_with_similar")]
        public List<CompareWithSimilar> CompareWithSimilar { get; set; }

        [JsonProperty("sponsored_products")]
        public List<CompareWithSimilar> SponsoredProducts { get; set; }
    }
}
