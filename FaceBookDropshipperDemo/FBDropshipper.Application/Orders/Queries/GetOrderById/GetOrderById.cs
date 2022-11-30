using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Orders.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdRequestModel : IRequest<GetOrderByIdResponseModel>
{
    public int Id { get; set; }
}

public class GetOrderByIdRequestModelValidator : AbstractValidator<GetOrderByIdRequestModel>
{
    public GetOrderByIdRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    GetOrderByIdRequestHandler : IRequestHandler<GetOrderByIdRequestModel, GetOrderByIdResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetOrderByIdRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetOrderByIdResponseModel> Handle(GetOrderByIdRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var order = await _context.Orders.GetByWithSelectAsync(p => p.MarketPlace.Team.UserId == userId && p.Id == request.Id,
            OrderSelector.Selector, cancellationToken: cancellationToken);
        if (order == null)
        {
            throw new NotFoundException(nameof(order));
        }
        return order.CreateCopy<GetOrderByIdResponseModel>();
    }

}

public class GetOrderByIdResponseModel : OrderDto
{

}