using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.Orders.Commands.DeleteOrderBulk;

public class DeleteOrderBulkRequestModel : IRequest<DeleteOrderBulkResponseModel>
{
    public int[] Ids { get; set; }
}

public class DeleteOrderBulkRequestModelValidator : AbstractValidator<DeleteOrderBulkRequestModel>
{
    public DeleteOrderBulkRequestModelValidator()
    {
        RuleFor(p => p.Ids).Required().Max(50);
    }
}

public class
    DeleteOrderBulkRequestHandler : IRequestHandler<DeleteOrderBulkRequestModel, DeleteOrderBulkResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;

    public DeleteOrderBulkRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<DeleteOrderBulkResponseModel> Handle(DeleteOrderBulkRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var orders = await _context.Orders.GetAll(p => request.Ids.Contains(p.Id) 
                                                       && p.MarketPlace.Team.UserId == userId)
            .ToListAsync(cancellationToken: cancellationToken);
        
        _context.Orders.RemoveRange(orders);
        await _context.SaveChangesAsync(cancellationToken);
        return new DeleteOrderBulkResponseModel()
        {
            Count = orders.Count
        };
    }

}

public class DeleteOrderBulkResponseModel
{
    public int Count { get; set; }
}