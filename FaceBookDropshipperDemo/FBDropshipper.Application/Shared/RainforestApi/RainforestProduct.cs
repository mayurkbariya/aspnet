using Newtonsoft.Json;

namespace FBDropshipper.Application.Shared.RainforestApi;

public class RainforestProduct
{
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("search_alias")]
    public SearchAlias SearchAlias { get; set; }

    [JsonProperty("keywords")]
    public string Keywords { get; set; }

    [JsonProperty("keywords_list")]
    public List<string> KeywordsList { get; set; }

    [JsonProperty("asin")]
    public string Asin { get; set; }

    [JsonProperty("parent_asin")]
    public string ParentAsin { get; set; }

    [JsonProperty("link")]
    public string Link { get; set; }

    [JsonProperty("brand")]
    public string Brand { get; set; }

    [JsonProperty("add_an_accessory")]
    public List<AddAnAccessory> AddAnAccessory { get; set; }

    [JsonProperty("sell_on_amazon")]
    public bool SellOnAmazon { get; set; }

    [JsonProperty("variants")]
    public List<Variant> Variants { get; set; }

    [JsonProperty("variant_asins_flat")]
    public string VariantAsinsFlat { get; set; }

    [JsonProperty("categories")]
    public List<Category> Categories { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("promotions_feature")]
    public string PromotionsFeature { get; set; }

    [JsonProperty("a_plus_content")]
    public APlusContent APlusContent { get; set; }

    [JsonProperty("sub_title")]
    public SubTitle SubTitle { get; set; }

    [JsonProperty("rating")]
    public double Rating { get; set; }

    [JsonProperty("rating_breakdown")]
    public RatingBreakdown RatingBreakdown { get; set; }

    [JsonProperty("ratings_total")]
    public long RatingsTotal { get; set; }

    [JsonProperty("reviews_total")]
    public long ReviewsTotal { get; set; }

    [JsonProperty("main_image")]
    public MainImageElement MainImage { get; set; }

    [JsonProperty("images")]
    public List<VariantImage> Images { get; set; }

    [JsonProperty("images_count")]
    public long ImagesCount { get; set; }

    [JsonProperty("videos")]
    public List<ProductVideo> Videos { get; set; }

    [JsonProperty("videos_count")]
    public long VideosCount { get; set; }

    [JsonProperty("is_bundle")]
    public bool IsBundle { get; set; }

    [JsonProperty("feature_bullets")]
    public List<string> FeatureBullets { get; set; }

    [JsonProperty("feature_bullets_count")]
    public long FeatureBulletsCount { get; set; }

    [JsonProperty("feature_bullets_flat")]
    public string FeatureBulletsFlat { get; set; }

    [JsonProperty("important_information")]
    public ImportantInformation ImportantInformation { get; set; }

    [JsonProperty("top_reviews")]
    public List<TopReview> TopReviews { get; set; }

    [JsonProperty("buybox_winner")]
    public BuyboxWinner BuyboxWinner { get; set; }

    [JsonProperty("more_buying_choices")]
    public List<MoreBuyingChoice> MoreBuyingChoices { get; set; }

    [JsonProperty("specifications")]
    public List<Ion> Specifications { get; set; }

    [JsonProperty("specifications_flat")]
    public string SpecificationsFlat { get; set; }

    [JsonProperty("bestsellers_rank")]
    public List<BestsellersRank> BestsellersRank { get; set; }

    [JsonProperty("material")]
    public string Material { get; set; }

    [JsonProperty("weight")]
    public string Weight { get; set; }

    [JsonProperty("dimensions")]
    public string Dimensions { get; set; }

    [JsonProperty("model_number")]
    public string ModelNumber { get; set; }

    [JsonProperty("bestsellers_rank_flat")]
    public string BestsellersRankFlat { get; set; }
}