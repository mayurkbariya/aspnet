using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Shared.RainforestApi;
using FBDropshipper.Infrastructure.Option;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FBDropshipper.Infrastructure.Service;

public class RainforestApiService : IRainforestApiService
{
    private readonly RainforestOption _options;
    private readonly HttpClient _httpClient;
    public RainforestApiService(IOptions<RainforestOption> options, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    private string GetUrl(string sku)
    {
        return
            $"https://api.rainforestapi.com/request?api_key={_options.ApiKey}&type=product&asin={sku}&amazon_domain=amazon.com";
    }


    public async Task<RainforestProductResponse> GetProductBySku(string sku)
    {
        var url = GetUrl(sku);
        var json = await _httpClient.GetStringAsync(url);
        return JsonConvert.DeserializeObject<RainforestProductResponse>(json);
    }
}