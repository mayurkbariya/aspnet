using FBDropshipper.Application.CatalogProductImages.Commands.CreateCatalogProductImage;
using FBDropshipper.Application.CatalogProductImages.Commands.DeleteCatalogProductImage;
using FBDropshipper.Application.CatalogProductImages.Commands.UpdateCatalogProductImageOrder;
using FBDropshipper.Application.CatalogProductImages.Queries.GetCatalogProductImages;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1;

public class CatalogProductImageController : BaseController
{
    /// <summary>
    /// Get Catalog Product Images
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProductImages.View)]
    [HttpGet("{id:int}")]
    public async Task<GetCatalogProductImagesResponseModel> GetCatalogProductImages([FromRoute] int id)
    {
        var model = new GetCatalogProductImagesRequestModel()
        {
            Id = id
        };
        return await Mediator.Send(model);
    }
    
    
    /// <summary>
    /// Create Catalog Product Images
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProductImages.Insert)]
    [HttpPost]
    public async Task<CreateCatalogProductImageResponseModel> CreateCatalogProductImage(CreateCatalogProductImageRequestModel model)
    {
        return await Mediator.Send(model);
    }
    /// <summary>
    /// Update Catalog Image Order
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProductImages.Update)]
    [HttpPut("{id:int}")]
    public async Task<UpdateCatalogProductImageOrderResponseModel> UpdateCatalogProductImageOrder([FromRoute] int id, UpdateCatalogProductImageOrderRequestModel model)
    {
        model.Id = id;
        return await Mediator.Send(model);
    }
    
    /// <summary>
    /// Delete Catalog Product Images
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.CatalogProductImages.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<DeleteCatalogProductImageResponseModel> DeleteCatalogProductImages([FromRoute] int id)
    {
        var model = new DeleteCatalogProductImageRequestModel()
        {
            Id = id
        };
        return await Mediator.Send(model);
    }

}