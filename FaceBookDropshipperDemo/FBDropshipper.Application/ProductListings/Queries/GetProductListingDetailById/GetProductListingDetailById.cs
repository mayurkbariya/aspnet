using System.Linq.Expressions;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.ProductListings.Models;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.ProductListings.Queries.GetProductListingDetailById;

public class GetProductListingDetailByIdRequestModel : IRequest<GetProductListingDetailByIdResponseModel>
{
    public int Id { get; set; }

}

public class
    GetProductListingDetailByIdRequestModelValidator : AbstractValidator<GetProductListingDetailByIdRequestModel>
{
    public GetProductListingDetailByIdRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    GetProductListingDetailByIdRequestHandler : IRequestHandler<GetProductListingDetailByIdRequestModel,
        GetProductListingDetailByIdResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetProductListingDetailByIdRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetProductListingDetailByIdResponseModel> Handle(GetProductListingDetailByIdRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        Expression<Func<ProductListing, bool>> query = p => 
            p.Id == request.Id && (
                p.MarketPlace.Team.UserId == userId);
        var product = await _context.ProductListings.GetByWithSelectAsync(query, ProductListingSelector.SelectorDetail, cancellationToken: cancellationToken);
        if (product == null)
        {
            throw new NotFoundException(nameof(product));
        }
        return new GetProductListingDetailByIdResponseModel()
        {
            Data = product
        };
    }

}

public class GetProductListingDetailByIdResponseModel
{
    public ProductListingDetailDto Data { get; set; }
}