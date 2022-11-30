using FBDropshipper.Application.ListingTemplates.Commands.CreateListingTemplate;
using FBDropshipper.Application.ListingTemplates.Commands.DeleteListingTemplate;
using FBDropshipper.Application.ListingTemplates.Commands.DeleteListingTemplateBulk;
using FBDropshipper.Application.ListingTemplates.Commands.UpdateListingTemplate;
using FBDropshipper.Application.ListingTemplates.Queries.GetListingTemplateById;
using FBDropshipper.Application.ListingTemplates.Queries.GetListingTemplates;
using FBDropshipper.Application.ListingTemplates.Queries.GetListingTemplatesDropDown;
using FBDropshipper.Application.ProductListings.Commands.ApplyTemplateToAllProducts;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1;

public class ListingTemplateController : BaseController
{
    /// <summary>
    /// Get ListingTemplates
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ListingTemplates.View)]
    [HttpGet]
    public async Task<GetListingTemplatesResponseModel> GetListingTemplate([FromQuery] GetListingTemplatesRequestModel model)
    {
        return await Mediator.Send(model);
    }
    /// <summary>
    /// Get Listing Template By Id
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ListingTemplates.View)]
    [HttpGet("{id:int}")]
    public async Task<GetListingTemplateByIdResponseModel> GetListingTemplatesById([FromRoute] int id)
    {
        var model = new GetListingTemplateByIdRequestModel()
        {
            Id = id
        };
        return await Mediator.Send(model);
    }
    
    /// <summary>
    /// Get Listing Template Dropdown
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ListingTemplates.View)]
    [HttpGet("dropDown/{id:int}")]
    public async Task<GetListingTemplateDropDownResponseModel> GetListingTemplatesDropDown([FromRoute] int id)
    {
        var model = new GetListingTemplateDropDownRequestModel()
        {
            MarketPlaceId = id
        };
        return await Mediator.Send(model);
    }


    /// <summary>
    /// Create Listing Template
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ListingTemplates.Insert)]
    [HttpPost]
    public async Task<CreateListingTemplateResponseModel> CreateListingTemplate(CreateListingTemplateRequestModel model)
    {
        return await Mediator.Send(model);
    }
    

    /// <summary>
    /// Delete Listing Template
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ListingTemplates.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<DeleteListingTemplateResponseModel> DeleteListingTemplate([FromRoute] int id)
    {
        var model = new DeleteListingTemplateRequestModel()
        {
            Id = id,
        };
        return await Mediator.Send(model);
    }
    
    
    
    /// <summary>
    /// Delete Listing Template in Bulk
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ListingTemplates.Delete)]
    [HttpPost("delete/bulk")]
    public async Task<DeleteListingTemplateBulkResponseModel> DeleteListingTemplateBulk(DeleteListingTemplateBulkRequestModel model)
    {
        return await Mediator.Send(model);
    }


    /// <summary>
    /// Update Listing Template
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.ListingTemplates.Update)]
    [HttpPut("{id:int}")]
    public async Task<UpdateListingTemplateResponseModel> UpdateListingTemplate([FromRoute] int id, UpdateListingTemplateRequestModel model)
    {
        model.Id = id;
        return await Mediator.Send(model);
    }
}