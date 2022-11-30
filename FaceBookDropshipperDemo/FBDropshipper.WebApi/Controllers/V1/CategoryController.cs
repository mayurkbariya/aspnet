using FBDropshipper.Application.Categories.Queries.GetCategoriesDropDown;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1;

public class CategoryController : BaseController
{
    /// <summary>
    /// Get Categories DropDown
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("dropDown")]
    public async Task<GetCategoriesDropDownResponseModel> GetCategoryDropDown()
    {
        return await Mediator.Send(new GetCategoriesDropDownRequestModel());
    }
}