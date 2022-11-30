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

namespace FBDropshipper.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderRequestModel : IRequest<UpdateOrderResponseModel>
{
    public int Id { get; set; }
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

public class UpdateOrderRequestModelValidator : AbstractValidator<UpdateOrderRequestModel>
{
    public UpdateOrderRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
        RuleFor(p => p.Quantity).Required();
        RuleFor(p => p.SubTotal).Required();
        RuleFor(p => p.Fee).Min(0);
        RuleFor(p => p.SupplierCost).Min(0);
        RuleFor(p => p.TrackingCarrier).IsInEnum();
        RuleFor(p => p.OrderStatus).IsInEnum();
    }
}

public class
    UpdateOrderRequestHandler : IRequestHandler<UpdateOrderRequestModel, UpdateOrderResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;

    public UpdateOrderRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<UpdateOrderResponseModel> Handle(UpdateOrderRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var order = await _context.Orders.GetByReadOnlyAsync(p => p.Id == request.Id && p.MarketPlace.Team.UserId == userId, cancellationToken: cancellationToken);
        if (order == null)
        {
            throw new CannotUpdateException(nameof(order));
        }

        order.Fee = request.Fee;
        order.Quantity = request.Quantity;
        order.SubTotal = request.SubTotal;
        order.OrderId = request.OrderId;
        order.OrderUrl = request.OrderUrl;
        order.Shipping = request.Shipping;
        order.SupplierOrderId = request.SupplierOrderId;
        order.SupplierCost = request.SupplierCost;
        order.TrackingCarrier = request.TrackingCarrier.ToInt();
        order.TrackingNumber = request.TrackingNumber;
        order.OrderStatus = request.OrderStatus.ToInt();
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
        return new UpdateOrderResponseModel(order);
    }

}

public class UpdateOrderResponseModel : OrderDto
{
    public UpdateOrderResponseModel(Order order) : base(order)
    {
    }
}