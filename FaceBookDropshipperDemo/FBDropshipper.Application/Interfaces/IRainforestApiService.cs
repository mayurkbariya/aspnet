using FBDropshipper.Application.Shared.RainforestApi;

namespace FBDropshipper.Application.Interfaces;

public interface IRainforestApiService
{
    Task<RainforestProductResponse> GetProductBySku(string sku);
}