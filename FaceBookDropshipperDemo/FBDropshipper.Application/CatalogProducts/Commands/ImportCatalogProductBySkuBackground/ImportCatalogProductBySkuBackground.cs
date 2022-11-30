using FBDropshipper.Application.CatalogProducts.Commands.ImportAndAddToInventory;
using FBDropshipper.Application.CatalogProducts.Commands.ImportCatalogProductBySku;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.CatalogProducts.Commands.ImportCatalogProductBySkuBackground;

public class
    ImportCatalogProductBySkuBackgroundRequestModel : IRequest<ImportCatalogProductBySkuBackgroundResponseModel>
{
    public int MarketPlaceId { get; set; }
    public string[] SkuCodes { get; set; }
}

public class
    ImportCatalogProductBySkuBackgroundRequestModelValidator : AbstractValidator<
        ImportCatalogProductBySkuBackgroundRequestModel>
{
    public ImportCatalogProductBySkuBackgroundRequestModelValidator()
    {
        RuleFor(p => p.SkuCodes).Required();
    }
}

public class
    ImportCatalogProductBySkuBackgroundRequestHandler : IRequestHandler<ImportCatalogProductBySkuBackgroundRequestModel,
        ImportCatalogProductBySkuBackgroundResponseModel>
{
    private readonly IBackgroundTaskQueueService _queueService;
    private readonly ISessionService _sessionService;
    private readonly ApplicationDbContext _applicationDbContext;
    public ImportCatalogProductBySkuBackgroundRequestHandler(IBackgroundTaskQueueService queueService, ISessionService sessionService, ApplicationDbContext applicationDbContext)
    {
        _queueService = queueService;
        _sessionService = sessionService;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<ImportCatalogProductBySkuBackgroundResponseModel> Handle(
        ImportCatalogProductBySkuBackgroundRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var sub = await _applicationDbContext.UserSubscriptions.GetByAsync(p =>
                p.UserId == userId && p.IsActive, 
            p => p.Include(pr => pr.Subscription),
            cancellationToken);
        if (sub == null)
        {
            throw new OkayButNotSuccessfulException("No Active Subscription Found");
        }

        var totalMembers = await _applicationDbContext.InventoryProducts.ActiveCount(p => p.CatalogProduct.Catalog.UserId == userId, cancellationToken);
        if (totalMembers + request.SkuCodes.Length >= sub.Subscription.TotalProducts)
        {
            throw new OkayButNotSuccessfulException("No Free Slots left for Products. Delete existing and try again");
        }
        _queueService.QueueBackgroundWorkItem(new ImportAndAddToInventoryRequestModel()
        {
            SkuCodes = request.SkuCodes,
            MarketPlaceId = request.MarketPlaceId,
            UserIds = _sessionService.GetAllUserIds(),
        });
        return await Task.FromResult(new ImportCatalogProductBySkuBackgroundResponseModel());
    }

}

public class ImportCatalogProductBySkuBackgroundResponseModel
{

}