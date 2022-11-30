using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.ListingTemplates.Models;
using FBDropshipper.Application.ProductListings.Commands.CreateProductListingBulk;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.ProductListings.Commands.CreateProductListingBackground;

public class CreateProductListingBackgroundRequestModel : IRequest<CreateProductListingBackgroundResponseModel>
{
    public int MarketPlaceId { get; set; }
    public int TemplateId { get; set; }
    public int[] InventoryProductIds { get; set; }
    public int CategoryId { get; set; }
}

public class
    CreateProductListingBackgroundRequestModelValidator : AbstractValidator<CreateProductListingBackgroundRequestModel>
{
    public CreateProductListingBackgroundRequestModelValidator()
    {
        RuleFor(p => p.MarketPlaceId).Required();
        RuleFor(p => p.TemplateId).Required();
        RuleFor(p => p.InventoryProductIds).Required().Max(50);
        RuleFor(p => p.CategoryId).Min(0);
    }
}

public class
    CreateProductListingBackgroundRequestHandler : IRequestHandler<CreateProductListingBackgroundRequestModel,
        CreateProductListingBackgroundResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IBackgroundTaskQueueService _queueService;
    public CreateProductListingBackgroundRequestHandler(ApplicationDbContext context, ISessionService sessionService, IBackgroundTaskQueueService queueService)
    {
        _context = context;
        _sessionService = sessionService;
        _queueService = queueService;
    }

    public async Task<CreateProductListingBackgroundResponseModel> Handle(
        CreateProductListingBackgroundRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        if (request.CategoryId > 0)
        {
            var category = await _context.Categories.GetByReadOnlyAsync(p => p.Id == request.CategoryId, cancellationToken: cancellationToken);
            if (category == null)
            {
                throw new NotFoundException(nameof(category));
            }
        }
        
        var template = _context.ListingTemplates.GetByReadOnly(p =>
            p.Id == request.TemplateId && p.MarketPlaceId == request.MarketPlaceId &&
            p.MarketPlace.Team.UserId == userId);
        if (template == null)
        {
            throw new NotFoundException(nameof(template));
        }
        _queueService.QueueBackgroundWorkItem(new CreateProductListingBulkRequestModel()
        {
            Template = new ListingTemplateDto(template),
            CategoryId = request.CategoryId,
            TemplateId = request.TemplateId,
            InventoryProductIds = request.InventoryProductIds,
            MarketPlaceId = request.MarketPlaceId
        });
        return new CreateProductListingBackgroundResponseModel();
    }

}

public class CreateProductListingBackgroundResponseModel
{

}