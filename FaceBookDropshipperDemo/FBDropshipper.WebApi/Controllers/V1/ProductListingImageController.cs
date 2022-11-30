using FBDropshipper.Application.CatalogProductImages.Commands.CreateCatalogProductImage;
using FBDropshipper.Application.ProductListingImages.Commands.CreateProductListingImage;
using FBDropshipper.Application.ProductListingImages.Commands.DeleteProductListingImage;
using FBDropshipper.Application.ProductListingImages.Commands.UpdateProductListingImageOrder;
using FBDropshipper.Application.ProductListingImages.Queries.GetProductListingImages;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1;

public class ProductListingImageController : BaseController
{
    /// <summary>
    /// Get Inventory Product Images
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ProductListingImages.View)]
    [HttpGet("{id:int}")]
    public async Task<GetProductListingImagesResponseModel> GetProductListingImages([FromRoute] int id)
    {
        var model = new GetProductListingImagesRequestModel()
        {
            Id = id
        };
        return await Mediator.Send(model);
    }
    
    
    
    /// <summary>
    /// Create Inventory Product Images
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ProductListingImages.Insert)]
    [HttpPost]
    public async Task<CreateProductListingImageResponseModel> CreateCatalogProductImage(CreateProductListingImageRequestModel model)
    {
        return await Mediator.Send(model);
    }
    
    /// <summary>
    /// Update Inventory Image Order
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ProductListingImages.Update)]
    [HttpPut("{id:int}")]
    public async Task<UpdateProductListingImageOrderResponseModel> UpdateProductListingImageOrder([FromRoute] int id, UpdateProductListingImageOrderRequestModel model)
    {
        model.Id = id;
        return await Mediator.Send(model);
    }
    /// <summary>
    /// Delete Inventory Product Images
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ProductListingImages.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<DeleteProductListingImageResponseModel> DeleteProductListingImages([FromRoute] int id)
    {
        var model = new DeleteProductListingImageRequestModel()
        {
            Id = id
        };
        return await Mediator.Send(model);
    }

}