using FBDropshipper.Application.AppTransactions.Queries.GetAppTransactions;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1;

public class AppTransactionController : BaseController
{
    /// <summary>
    /// Get App Transactions
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Roles = RoleNames.Admin + "," + RoleNames.TeamLeader)]
    [HttpGet]
    public async Task<GetAppTransactionsResponseModel> GetAppTransactions([FromQuery] GetAppTransactionsRequestModel model)
    {
        return await Mediator.Send(model);
    }
}