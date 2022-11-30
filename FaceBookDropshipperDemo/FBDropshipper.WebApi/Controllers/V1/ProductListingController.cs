using FBDropshipper.Application.ProductListings.Commands.ApplyCategoryToAllProducts;
using FBDropshipper.Application.ProductListings.Commands.ApplyTemplateToAllProducts;
using FBDropshipper.Application.ProductListings.Commands.BulkUpdateProductListingStatus;
using FBDropshipper.Application.ProductListings.Commands.CreateProductListing;
using FBDropshipper.Application.ProductListings.Commands.CreateProductListingBackground;
using FBDropshipper.Application.ProductListings.Commands.DeleteProductListing;
using FBDropshipper.Application.ProductListings.Commands.DeleteProductListingBulk;
using FBDropshipper.Application.ProductListings.Commands.UpdateProductListing;
using FBDropshipper.Application.ProductListings.Queries.GetProductListingDetailById;
using FBDropshipper.Application.ProductListings.Queries.GetProductListings;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1;

public class ProductListingController : BaseController
{
    /// <summary>
    /// Get Product Listings
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ProductListings.View)]
    [HttpGet]
    public async Task<GetProductListingsResponseModel> GetProductListing([FromQuery] GetProductListingsRequestModel model)
    {
        return await Mediator.Send(model);
    }

    

    /// <summary>
    /// Create Product Listings
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ProductListings.Insert)]
    [HttpPost]
    public async Task<CreateProductListingResponseModel> CreateProductListing(CreateProductListingRequestModel model)
    {
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Apply Listing Template to Products
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ProductListings.Update)]
    [HttpPut("apply/template/{id:int}")]
    public async Task<ApplyTemplateToAllProductsResponseModel> ApplyTemplateToAllProducts([FromRoute] int id,ApplyTemplateToAllProductsRequestModel model)
    {
        model.Id = id;
        return await Mediator.Send(model);
    }
    
    /// <summary>
    /// Bulk Update Product Listing Status
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ProductListings.Update)]
    [HttpPut("status/bulk")]
    public async Task<BulkUpdateProductListingStatusResponseModel> BulkUpdateProductListingStatus(BulkUpdateProductListingStatusRequestModel model)
    {
        return await Mediator.Send(model);
    }
    /// <summary>
    /// Apply Listing Template to Products
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ProductListings.Update)]
    [HttpPut("apply/category/{id:int}")]
    public async Task<ApplyCategoryToAllProductsResponseModel> ApplyCategoryToAllProducts([FromRoute] int id,ApplyCategoryToAllProductsRequestModel model)
    {
        model.Id = id;
        return await Mediator.Send(model);
    }
    /// <summary>
    /// Create Product Listings in bulk
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ProductListings.Insert)]
    [HttpPost("bulk")]
    public async Task<CreateProductListingBackgroundResponseModel> CreateProductListing(CreateProductListingBackgroundRequestModel model)
    {
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Delete Product Listing
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ProductListings.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<DeleteProductListingResponseModel> DeleteProductListing([FromRoute] int id)
    {
        var model = new DeleteProductListingRequestModel()
        {
            Id = id,
        };
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Delete Product Listing in Bulk
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ProductListings.Delete)]
    [HttpPost("delete/bulk")]
    public async Task<DeleteProductListingBulkResponseModel> DeleteProductListing(DeleteProductListingBulkRequestModel model)
    {
        return await Mediator.Send(model);
    }
    /// <summary>
    /// Update Product Listing
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ProductListings.Update)]
    [HttpPut("{id:int}")]
    public async Task<UpdateProductListingResponseModel> UpdateProductListing([FromRoute] int id, UpdateProductListingRequestModel model)
    {
        model.Id = id;
        return await Mediator.Send(model);
    }
    /// <summary>
    /// Get Product Listing Detail
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ProductListings.View)]
    [HttpGet("detail/{id:int}")]
    public async Task<GetProductListingDetailByIdResponseModel> GetProductListingDetail([FromRoute] int id)
    {
        var model = new GetProductListingDetailByIdRequestModel()
        {
            Id = id
        };
        return await Mediator.Send(model);
    }
}