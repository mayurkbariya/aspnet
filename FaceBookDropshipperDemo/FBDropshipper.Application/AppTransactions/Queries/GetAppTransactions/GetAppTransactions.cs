using System.Linq.Expressions;
using FBDropshipper.Application.AppTransactions.Models;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Shared;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Constant;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.AppTransactions.Queries.GetAppTransactions;

public class GetAppTransactionsRequestModel : GetPagedRequest<GetAppTransactionsResponseModel>
{
    
}

public class GetAppTransactionsRequestModelValidator : PageRequestValidator<GetAppTransactionsRequestModel>
{
    public GetAppTransactionsRequestModelValidator()
    {

    }
}

public class
    GetAppTransactionsRequestHandler : IRequestHandler<GetAppTransactionsRequestModel, GetAppTransactionsResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetAppTransactionsRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetAppTransactionsResponseModel> Handle(GetAppTransactionsRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetUserId();
        var role = _sessionService.GetRole();
        Expression<Func<AppTransaction, bool>> query = p => p.UserId == userId;
        bool isAdmin = false;
        if (role == RoleNames.Admin)
        {
            isAdmin = true;
            query = p => true;
        }
        if (request.Search.IsNotNullOrWhiteSpace())
        {
            query = query.AndAlso(p => 
                p.StripePaymentId.ToLower().Contains(request.Search) ||
                p.Status.ToLower().Contains(request.Search));
        }

        var list = await _context.AppTransactions.GetManyReadOnly(query, request)
            .Select(
                isAdmin ? AppTransactionSelector.SelectorAdmin : AppTransactionSelector.Selector)
            .ToListAsync(cancellationToken: cancellationToken);
        var count = await _context.AppTransactions.ActiveCount(query, cancellationToken);
        return new GetAppTransactionsResponseModel()
        {
            Data = list,
            Count = count
        };
    }

}

public class GetAppTransactionsResponseModel
{
    public List<AppTransactionDto> Data { get; set; }
    public int Count { get; set; }
}