using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Orders.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Orders.Commands.CreateOrder;

public class CreateOrderRequestModel : IRequest<CreateOrderResponseModel>
{
    public int MarketPlaceId { get; set; }
    public int ProductListingId { get; set; }
    public string OrderId { get; set; }
    public string OrderUrl { get; set; }
    public int Quantity { get; set; }
    public double SubTotal { get; set; }
    public double Shipping { get; set; }
    public double Fee { get; set; }
    public string SupplierOrderId { get; set; }
    public double SupplierCost { get; set; }
    public TrackingCarrier TrackingCarrier { get; set; }
    public string TrackingNumber { get; set; }
    public OrderStatus OrderStatus { get; set; }
}

public class CreateOrderRequestModelValidator : AbstractValidator<CreateOrderRequestModel>
{
    public CreateOrderRequestModelValidator()
    {
        RuleFor(p => p.MarketPlaceId).Required();
        RuleFor(p => p.ProductListingId).Required();
        RuleFor(p => p.Quantity).Required();
        RuleFor(p => p.SubTotal).Required();
        RuleFor(p => p.Fee).Min(0);
        RuleFor(p => p.SupplierCost).Min(0);
        RuleFor(p => p.TrackingCarrier).IsInEnum();
        RuleFor(p => p.OrderStatus).IsInEnum();
        RuleFor(p => p.Shipping).Min(0);
    }
}

public class
    CreateOrderRequestHandler : IRequestHandler<CreateOrderRequestModel, CreateOrderResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public CreateOrderRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<CreateOrderResponseModel> Handle(CreateOrderRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var marketPlace = await 
            _context.MarketPlaces.GetByReadOnlyAsync(p => p.Id == request.MarketPlaceId && p.Team.UserId == userId, cancellationToken: cancellationToken);
        if (marketPlace == null)
        {
            throw new NotFoundException(nameof(marketPlace));
        }

        var productListing = await _context.ProductListings.GetByReadOnlyAsync(p =>
            p.Id == request.ProductListingId && p.MarketPlaceId == request.MarketPlaceId, cancellationToken: cancellationToken);
        if (productListing == null)
        {
            throw new NotFoundException(nameof(marketPlace));
        }

        var order = new Order()
        {
            Fee = request.Fee,
            Quantity = request.Quantity,
            Shipping = request.Shipping,
            OrderId = request.OrderId,
            OrderStatus = request.OrderStatus.ToInt(),
            OrderUrl = request.OrderUrl,
            SupplierOrderId = request.SupplierOrderId,
            ProductListingId = request.ProductListingId,
            MarketPlaceId = request.MarketPlaceId,
            TrackingNumber = request.TrackingNumber,
            TrackingCarrier = request.TrackingCarrier.ToInt(),
            SupplierCost = request.SupplierCost,
            SubTotal = request.SubTotal,
        };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        order.ProductListing = productListing;
        order.MarketPlace = marketPlace;
        return new CreateOrderResponseModel(order);
    }

}

public class CreateOrderResponseModel : OrderDto
{
    public CreateOrderResponseModel(Order order) : base(order)
    {
    }
}