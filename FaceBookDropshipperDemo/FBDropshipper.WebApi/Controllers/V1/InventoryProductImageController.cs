using FBDropshipper.Application.CatalogProductImages.Commands.CreateCatalogProductImage;
using FBDropshipper.Application.InventoryProductImages.Commands.CreateInventoryProductImage;
using FBDropshipper.Application.InventoryProductImages.Commands.DeleteInventoryProductImage;
using FBDropshipper.Application.InventoryProductImages.Commands.UpdateInventoryProductImageOrder;
using FBDropshipper.Application.InventoryProductImages.Queries.GetInventoryProductImages;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1;

public class InventoryProductImageController : BaseController
{
    /// <summary>
    /// Get Inventory Product Images
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.InventoryProductImages.View)]
    [HttpGet("{id:int}")]
    public async Task<GetInventoryProductImagesResponseModel> GetInventoryProductImages([FromRoute] int id)
    {
        var model = new GetInventoryProductImagesRequestModel()
        {
            Id = id
        };
        return await Mediator.Send(model);
    }
    
    
    
    /// <summary>
    /// Create Inventory Product Images
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.InventoryProductImages.Insert)]
    [HttpPost]
    public async Task<CreateInventoryProductImageResponseModel> CreateCatalogProductImage(CreateInventoryProductImageRequestModel model)
    {
        return await Mediator.Send(model);
    }
    
    /// <summary>
    /// Update Inventory Image Order
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.InventoryProductImages.Update)]
    [HttpPut("{id:int}")]
    public async Task<UpdateInventoryProductImageOrderResponseModel> UpdateInventoryProductImageOrder([FromRoute] int id, UpdateInventoryProductImageOrderRequestModel model)
    {
        model.Id = id;
        return await Mediator.Send(model);
    }
    /// <summary>
    /// Delete Inventory Product Images
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.InventoryProductImages.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<DeleteInventoryProductImageResponseModel> DeleteInventoryProductImages([FromRoute] int id)
    {
        var model = new DeleteInventoryProductImageRequestModel()
        {
            Id = id
        };
        return await Mediator.Send(model);
    }

}