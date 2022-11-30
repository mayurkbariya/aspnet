using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.ProductListings.Commands.ApplyTemplateToAllProducts;

public class ApplyTemplateToAllProductsRequestModel : IRequest<ApplyTemplateToAllProductsResponseModel>
{
    public int Id { get; set; }
    public int[] ProductIds { get; set; }
}

public class ApplyTemplateToAllProductsRequestModelValidator : AbstractValidator<ApplyTemplateToAllProductsRequestModel>
{
    public ApplyTemplateToAllProductsRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
        RuleFor(p => p.ProductIds).Required().Max(50);
    }
}

public class
    ApplyTemplateToAllProductsRequestHandler : IRequestHandler<ApplyTemplateToAllProductsRequestModel,
        ApplyTemplateToAllProductsResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;

    public ApplyTemplateToAllProductsRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<ApplyTemplateToAllProductsResponseModel> Handle(ApplyTemplateToAllProductsRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var template =
            _context.ListingTemplates.GetByReadOnly(p =>
                p.Id == request.Id && p.MarketPlace.Team.UserId == userId);
        if (template == null)
        {
            throw new NotFoundException(nameof(template));
        }

        var products = await _context.ProductListings.GetAll(p =>
            request.ProductIds.Contains(p.Id) && p.MarketPlace.Team.UserId == userId)
            .Include(pr => pr.InventoryProduct)
            .ToListAsync(cancellationToken: cancellationToken);
        foreach (var product in products)
        {
            product.Price = (float)Math.Ceiling(((template.ProfitPercent + 100) / 100) * product.InventoryProduct.Price);
            product.Header = template.Header;
            product.Quantity = template.Quantity;
            product.DeliveryMethod = template.DeliveryMethod;
            product.ShippingRate = template.ShippingRate;
        }
        _context.ProductListings.UpdateRange(products);
        await _context.SaveChangesAsync(cancellationToken);
        return new ApplyTemplateToAllProductsResponseModel()
        {
            Updated = products.Count
        };
    }

}

public class ApplyTemplateToAllProductsResponseModel
{
    public int Updated { get; set; }
}