using FBDropshipper.Application.CatalogProducts.Commands.AddToInventory;
using FBDropshipper.Application.CatalogProducts.Commands.ImportCatalogProductBySku;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Notifications.Commands.CreateNotification;
using FBDropshipper.Domain.Enum;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FBDropshipper.Application.CatalogProducts.Commands.ImportAndAddToInventory;

public class ImportAndAddToInventoryRequestModel : IRequest<ImportAndAddToInventoryResponseModel>
{
    public string[] UserIds { get; set; }
    public int MarketPlaceId { get; set; }
    public string[] SkuCodes { get; set; }
}

public class ImportAndAddToInventoryRequestModelValidator : AbstractValidator<ImportAndAddToInventoryRequestModel>
{
    public ImportAndAddToInventoryRequestModelValidator()
    {
        RuleFor(p => p.MarketPlaceId).Required();
        RuleFor(p => p.SkuCodes).Required().Max(50);
    }
}

public class
    ImportAndAddToInventoryRequestHandler : IRequestHandler<ImportAndAddToInventoryRequestModel,
        ImportAndAddToInventoryResponseModel>
{
    private readonly IMediator _mediator;
    private readonly ILogger<ImportAndAddToInventoryRequestHandler> _logger;
    public ImportAndAddToInventoryRequestHandler(IMediator mediator, ILogger<ImportAndAddToInventoryRequestHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<ImportAndAddToInventoryResponseModel> Handle(ImportAndAddToInventoryRequestModel request,
        CancellationToken cancellationToken)
    {
        var totalCount = request.SkuCodes.Length;
        var successCount = 0;
        foreach (var skuCode in request.SkuCodes)
        {
            try
            {
                var result = await _mediator.Send(new ImportCatalogProductBySkuRequestModel()
                {
                    SkuCode = skuCode
                }, cancellationToken);
                await _mediator.Send(new AddToInventoryRequestModel()
                {
                    CatalogProductId = result.Id,
                    MarketPlaceId = request.MarketPlaceId
                }, cancellationToken);
                successCount++;
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
            }
        }
        await _mediator.Send(new CreateNotificationRequestModel()
        {
            Message = "Importing of SKU has been finished",
            Data = new { totalCount, successCount },
            Type = NotificationType.ImportResult,
            UserIds = request.UserIds
        }, cancellationToken);
        return new ImportAndAddToInventoryResponseModel();
    }

}

public class ImportAndAddToInventoryResponseModel
{

}