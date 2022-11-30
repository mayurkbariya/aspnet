using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.ProductListings.Commands.BulkUpdateProductListingStatus;

public class BulkUpdateProductListingStatusRequestModel : IRequest<BulkUpdateProductListingStatusResponseModel>
{
    public ListingStatus Status { get; set; }
    public int[] ProductIds { get; set; }
}

public class
    BulkUpdateProductListingStatusRequestModelValidator : AbstractValidator<BulkUpdateProductListingStatusRequestModel>
{
    public BulkUpdateProductListingStatusRequestModelValidator()
    {
        RuleFor(p => p.Status).IsInEnum();
        RuleFor(p => p.ProductIds).Required().Max(50);
    }
}

public class
    BulkUpdateProductListingStatusRequestHandler : IRequestHandler<BulkUpdateProductListingStatusRequestModel,
        BulkUpdateProductListingStatusResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public BulkUpdateProductListingStatusRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<BulkUpdateProductListingStatusResponseModel> Handle(
        BulkUpdateProductListingStatusRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetUserId();
        var products = await _context.ProductListings
            .GetAll(p => p.MarketPlace.Team.UserId == userId && request.ProductIds.Contains(p.Id))
            .ToListAsync(cancellationToken: cancellationToken);
        foreach (var product in products)
        {
            product.ListingStatus = request.Status.ToInt();
        }
        _context.ProductListings.UpdateRange(products);
        await _context.SaveChangesAsync(cancellationToken);
        return new BulkUpdateProductListingStatusResponseModel()
        {
            Count = products.Count
        };
    }

}

public class BulkUpdateProductListingStatusResponseModel
{
    public int Count { get; set; }
}