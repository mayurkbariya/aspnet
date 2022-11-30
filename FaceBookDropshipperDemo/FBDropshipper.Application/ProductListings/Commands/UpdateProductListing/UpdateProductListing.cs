using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.ProductListings.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.ProductListings.Commands.UpdateProductListing;

public class UpdateProductListingRequestModel : IRequest<UpdateProductListingResponseModel>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Header { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public string ListingId { get; set; }
    public string ListingUrl { get; set; }
    public DateTime? ListedAt { get; set; }
    public int DeliveryMethod { get; set; }
    public ListingStatus ListingStatus { get; set; }
    public double ShippingRate { get; set; }
}

public class UpdateProductListingRequestModelValidator : AbstractValidator<UpdateProductListingRequestModel>
{
    public UpdateProductListingRequestModelValidator()
    {
        RuleFor(p => p.Title).Required().Max(255);
        RuleFor(p => p.ShippingRate).Min(0);
        RuleFor(p => p.Id).Required();
        RuleFor(p => p.Description).Required();
        RuleFor(p => p.Header).Required().Max(255);
        RuleFor(p => p.Price).Required();
        RuleFor(p => p.Quantity).Required();
        RuleFor(p => p.CategoryId).Min(0);
        RuleFor(p => p.DeliveryMethod).Required();
        RuleFor(p => p.ListingStatus).IsInEnum();
    }
}

public class
    UpdateProductListingRequestHandler : IRequestHandler<UpdateProductListingRequestModel,
        UpdateProductListingResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public UpdateProductListingRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<UpdateProductListingResponseModel> Handle(UpdateProductListingRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var product = await _context.ProductListings.GetByReadOnlyAsync(p =>
                p.Id == request.Id && (p.MarketPlace.Team.UserId == userId),
            p => p.Include(pr => pr.ProductListingImages),
            cancellationToken:cancellationToken);
        if (product == null)
        {
            throw new CannotDeleteException(nameof(product));
        }
        product.Title = request.Title;
        product.Description = request.Description;
        product.Header = request.Header;
        product.Price = request.Price;
        product.Quantity = request.Quantity;
        product.ListedAt = request.ListedAt;
        product.ListingId = request.ListingId;
        product.ListingUrl = request.ListingUrl;
        product.ListingStatus = request.ListingStatus.ToInt();
        product.DeliveryMethod = request.DeliveryMethod;
        product.ShippingRate = request.ShippingRate;
        if (product.CategoryId != request.CategoryId && request.CategoryId > 0)
        {
            var category = await _context.Categories.GetByReadOnlyAsync(p => p.Id == request.CategoryId, cancellationToken: cancellationToken);
            if (category == null)
            {
                throw new NotFoundException(nameof(category));
            }
            product.CategoryId = request.CategoryId;
        }
        _context.ProductListings.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
        return new UpdateProductListingResponseModel(product);
    }

}

public class UpdateProductListingResponseModel : ProductListingDetailDto
{
    public UpdateProductListingResponseModel(ProductListing product) : base(product)
    {
    }
}