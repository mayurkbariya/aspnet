using FBDropshipper.Application.InventoryProducts.Commands.DeleteInventoryProduct;
using FBDropshipper.Application.InventoryProducts.Commands.DeleteInventoryProductBulk;
using FBDropshipper.Application.InventoryProducts.Commands.UpdateInventoryProduct;
using FBDropshipper.Application.InventoryProducts.Queries.GetInventoryProductDetailById;
using FBDropshipper.Application.InventoryProducts.Queries.GetInventoryProducts;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1;

public class InventoryProductController : BaseController
{
    /// <summary>
    /// Get Inventory Products
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.InventoryProducts.View)]
    [HttpGet]
    public async Task<GetInventoryProductsResponseModel> GetInventoryProduct([FromQuery] GetInventoryProductsRequestModel model)
    {
        return await Mediator.Send(model);
    }

    

  

    /// <summary>
    /// Delete Inventory Product
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.InventoryProducts.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<DeleteInventoryProductResponseModel> DeleteInventoryProduct([FromRoute] int id)
    {
        var model = new DeleteInventoryProductRequestModel()
        {
            Id = id,
        };
        return await Mediator.Send(model);
    }
    
    /// <summary>
    /// Delete Inventory Products In Bulk
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.InventoryProducts.Delete)]
    [HttpPost("delete/bulk")]
    public async Task<DeleteInventoryProductBulkResponseModel> DeleteInventoryProductBulk(DeleteInventoryProductBulkRequestModel model)
    {
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Update Inventory Product
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.InventoryProducts.Update)]
    [HttpPut("{id:int}")]
    public async Task<UpdateInventoryProductResponseModel> UpdateInventoryProduct([FromRoute] int id, UpdateInventoryProductRequestModel model)
    {
        model.Id = id;
        return await Mediator.Send(model);
    }
    /// <summary>
    /// Get Inventory Product Detail
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.InventoryProducts.View)]
    [HttpPut("detail/{id:int}")]
    public async Task<GetInventoryProductDetailByIdResponseModel> GetInventoryProductDetail([FromRoute] int id)
    {
        var model = new GetInventoryProductDetailByIdRequestModel()
        {
            Id = id
        };
        return await Mediator.Send(model);
    }
}