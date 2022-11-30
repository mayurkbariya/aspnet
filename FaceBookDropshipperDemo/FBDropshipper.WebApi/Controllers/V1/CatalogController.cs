using FBDropshipper.Application.Catalogs.Commands.CreateCatalog;
using FBDropshipper.Application.Catalogs.Commands.DeleteCatalog;
using FBDropshipper.Application.Catalogs.Commands.UpdateCatalog;
using FBDropshipper.Application.Catalogs.Queries.GetCatalogById;
using FBDropshipper.Application.Catalogs.Queries.GetCatalogDropDown;
using FBDropshipper.Application.Catalogs.Queries.GetCatalogs;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1;

public class CatalogController : BaseController
{
    /// <summary>
    /// Get Catalogs
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Catalogs.View)]
    [HttpGet]
    public async Task<GetCatalogsResponseModel> GetCatalog([FromQuery] GetCatalogsRequestModel model)
    {
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Get Catalog By Id
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Catalogs.View)]
    [HttpGet("{id:int}")]
    public async Task<GetCatalogByIdResponseModel> GetCatalogById([FromRoute] int id)
    {
        var model = new GetCatalogByIdRequestModel()
        {
            Id = id
        };
        return await Mediator.Send(model);
    }
    /// <summary>
    /// Get Catalogs Dropdown
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Catalogs.View)]
    [HttpGet("dropDown")]
    public async Task<GetCatalogDropDownResponseModel> GetCatalogsDropDown()
    {
        var model = new GetCatalogDropDownRequestModel();
        return await Mediator.Send(model);
    }


    /// <summary>
    /// Create Catalog
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Catalogs.Insert)]
    [HttpPost]
    public async Task<CreateCatalogResponseModel> CreateCatalog(CreateCatalogRequestModel model)
    {
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Delete Catalog
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Catalogs.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<DeleteCatalogResponseModel> DeleteCatalog([FromRoute] int id)
    {
        var model = new DeleteCatalogRequestModel()
        {
            Id = id,
        };
        return await Mediator.Send(model);
    }


    /// <summary>
    /// Update Catalog
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Catalogs.Update)]
    [HttpPut("{id:int}")]
    public async Task<UpdateCatalogResponseModel> UpdateCatalog([FromRoute] int id, UpdateCatalogRequestModel model)
    {
        model.Id = id;
        return await Mediator.Send(model);
    }
}