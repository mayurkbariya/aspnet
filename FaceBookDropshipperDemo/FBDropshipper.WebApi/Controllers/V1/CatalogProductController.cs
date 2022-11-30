using FBDropshipper.Application.CatalogProducts.Commands.AddToInventory;
using FBDropshipper.Application.CatalogProducts.Commands.CreateCatalogProduct;
using FBDropshipper.Application.CatalogProducts.Commands.CreateCatalogProductWithImage;
using FBDropshipper.Application.CatalogProducts.Commands.DeleteCatalogProduct;
using FBDropshipper.Application.CatalogProducts.Commands.DeleteCatalogProductBulk;
using FBDropshipper.Application.CatalogProducts.Commands.ImportCatalogProductBySku;
using FBDropshipper.Application.CatalogProducts.Commands.ImportCatalogProductBySkuBackground;
using FBDropshipper.Application.CatalogProducts.Commands.UpdateCatalogProduct;
using FBDropshipper.Application.CatalogProducts.Queries.CheckIfCatalogProductExistBySku;
using FBDropshipper.Application.CatalogProducts.Queries.GetCatalogProductDetailById;
using FBDropshipper.Application.CatalogProducts.Queries.GetCatalogProducts;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1;

public class CatalogProductController : BaseController
{
    /// <summary>
    /// Get Catalog Products
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProducts.View)]
    [HttpGet]
    public async Task<GetCatalogProductsResponseModel> GetCatalogProduct([FromQuery] GetCatalogProductsRequestModel model)
    {
        return await Mediator.Send(model);
    }

    

    /// <summary>
    /// Create Catalog Product
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProducts.Insert)]
    [HttpPost]
    public async Task<CreateCatalogProductResponseModel> CreateCatalogProduct(CreateCatalogProductRequestModel model)
    {
        return await Mediator.Send(model);
    }
    /// <summary>
    /// Create Catalog Product with Image
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProducts.Insert)]
    [HttpPost("image")]
    public async Task<CreateCatalogProductWithImageResponseModel> CreateCatalogProductWithImage(CreateCatalogProductWithImageRequestModel model)
    {
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Delete Catalog Product
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProducts.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<DeleteCatalogProductResponseModel> DeleteCatalogProduct([FromRoute] int id)
    {
        var model = new DeleteCatalogProductRequestModel()
        {
            Id = id,
        };
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Delete Catalog Product in Bulk
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProducts.Delete)]
    [HttpPost("delete/bulk")]
    public async Task<DeleteCatalogProductBulkResponseModel> DeleteCatalogProduct(DeleteCatalogProductBulkRequestModel model)
    {
        return await Mediator.Send(model);
    }
    /// <summary>
    /// Update Catalog Product
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProducts.Update)]
    [HttpPut("{id:int}")]
    public async Task<UpdateCatalogProductResponseModel> UpdateCatalogProduct([FromRoute] int id, UpdateCatalogProductRequestModel model)
    {
        model.Id = id;
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Add Catalog Product to Inventory
    /// </summary>
    /// <param name="id"></param>
    /// <param name="marketplaceId"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProducts.AddToInventory)]
    [HttpPut("{id:int}/addToInventory/{marketplaceId:int}/marketplace")]
    public async Task<AddToInventoryResponseModel> AddToInventory([FromRoute] int id,[FromRoute] int marketplaceId)
    {
        var model = new AddToInventoryRequestModel
        {
            CatalogProductId = id,
            MarketPlaceId = marketplaceId
        };
        return await Mediator.Send(model);
    }
    
    /// <summary>
    /// Import Catalog Product
    /// </summary>
    /// <param name="sku"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProducts.Import)]
    [HttpPut("import/{sku}")]
    public async Task<ImportCatalogProductBySkuResponseModel> ImportCatalogProduct([FromRoute] string sku)
    {
        var model = new ImportCatalogProductBySkuRequestModel()
        {
            SkuCode = sku
        };
        return await Mediator.Send(model);
    }
    /// <summary>
    /// Import Catalog Product Background
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProducts.Import)]
    [HttpPut("import")]
    public async Task<ImportCatalogProductBySkuBackgroundResponseModel> ImportCatalogProductBackground(ImportCatalogProductBySkuBackgroundRequestModel model)
    {
        return await Mediator.Send(model);
    }
    
    /// <summary>
    /// Get Catalog Product Detail
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProducts.View)]
    [HttpPut("detail/{id:int}")]
    public async Task<GetCatalogProductDetailByIdResponseModel> GetCatalogProductDetail([FromRoute] int id)
    {
        var model = new GetCatalogProductDetailByIdRequestModel()
        {
            Id = id
        };
        return await Mediator.Send(model);
    }
    
    
    
    /// <summary>
    /// Check If Catalog Product Exists By Sku
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProducts.View)]
    [HttpPut("exists/{sku}")]
    public async Task<CheckIfCatalogProductExistBySkuResponseModel> CheckIfCatalogProductExistBySku([FromRoute] string sku)
    {
        var model = new CheckIfCatalogProductExistBySkuRequestModel()
        {
            SkuCode = sku
        };
        return await Mediator.Send(model);
    }
}