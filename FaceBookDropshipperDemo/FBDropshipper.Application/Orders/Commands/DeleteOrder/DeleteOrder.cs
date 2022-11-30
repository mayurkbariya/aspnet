using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderRequestModel : IRequest<DeleteOrderResponseModel>
{
    public int Id { get; set; }
}

public class DeleteOrderRequestModelValidator : AbstractValidator<DeleteOrderRequestModel>
{
    public DeleteOrderRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    DeleteOrderRequestHandler : IRequestHandler<DeleteOrderRequestModel, DeleteOrderResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public DeleteOrderRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<DeleteOrderResponseModel> Handle(DeleteOrderRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetUserId();
        var order = await _context.Orders.GetByReadOnlyAsync(p => p.Id == request.Id && p.MarketPlace.Team.UserId == userId, cancellationToken: cancellationToken);
        if (order == null)
        {
            throw new CannotDeleteException(nameof(order));
        }
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);
        return new DeleteOrderResponseModel();
    }

}

public class DeleteOrderResponseModel
{

}