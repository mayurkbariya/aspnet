using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.ProductListings.Commands.ApplyTemplateToAllProducts;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.ProductListings.Commands.ApplyCategoryToAllProducts;

public class ApplyCategoryToAllProductsRequestModel : IRequest<ApplyCategoryToAllProductsResponseModel>
{
    public int Id { get; set; }
    public int[] ProductIds { get; set; }
}

public class ApplyCategoryToAllProductsRequestModelValidator : AbstractValidator<ApplyCategoryToAllProductsRequestModel>
{
    public ApplyCategoryToAllProductsRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
        RuleFor(p => p.ProductIds).Required().Max(50);
    }
}

public class
    ApplyCategoryToAllProductsRequestHandler : IRequestHandler<ApplyCategoryToAllProductsRequestModel,
        ApplyCategoryToAllProductsResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public ApplyCategoryToAllProductsRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<ApplyCategoryToAllProductsResponseModel> Handle(ApplyCategoryToAllProductsRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var category =
            _context.Categories.GetByReadOnly(p =>
                p.Id == request.Id);
        if (category == null)
        {
            throw new NotFoundException(nameof(category));
        }

        var products = await _context.ProductListings.GetAll(p =>
                request.ProductIds.Contains(p.Id) && p.MarketPlace.Team.UserId == userId)
            .ToListAsync(cancellationToken: cancellationToken);
        foreach (var product in products)
        {
            product.CategoryId = category.Id;
        }
        _context.ProductListings.UpdateRange(products);
        await _context.SaveChangesAsync(cancellationToken);
        return new ApplyCategoryToAllProductsResponseModel()
        {
            Updated = products.Count
        };
    }

}

public class ApplyCategoryToAllProductsResponseModel
{
    public int Updated { get; set; }
}