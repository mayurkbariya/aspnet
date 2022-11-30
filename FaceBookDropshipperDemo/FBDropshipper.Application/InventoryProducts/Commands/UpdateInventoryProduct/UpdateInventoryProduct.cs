using FBDropshipper.Application.InventoryProducts.Models;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.InventoryProducts.Commands.UpdateInventoryProduct;

public class UpdateInventoryProductRequestModel : IRequest<UpdateInventoryProductResponseModel>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string SkuCode { get; set; }
    public string Url { get; set; }
    public StockStatus StockStatus { get; set; }
    public int Stock { get; set; }
    public double Price { get; set; }
}

public class UpdateInventoryProductRequestModelValidator : AbstractValidator<UpdateInventoryProductRequestModel>
{
    public UpdateInventoryProductRequestModelValidator()
    {
        RuleFor(p => p.Price).Required();
        RuleFor(p => p.Id).Required();
        RuleFor(p => p.Title).Required().Max(255);
        RuleFor(p => p.Description).Required();
        RuleFor(p => p.SkuCode).Required().Max(255);
        RuleFor(p => p.StockStatus).IsInEnum();
        RuleFor(p => p.Stock).Min(0);
    }
}

public class
    UpdateInventoryProductRequestHandler : IRequestHandler<UpdateInventoryProductRequestModel,
        UpdateInventoryProductResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public UpdateInventoryProductRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<UpdateInventoryProductResponseModel> Handle(UpdateInventoryProductRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var product = await _context.InventoryProducts.GetByReadOnlyAsync(p => 
            p.Id == request.Id && (p.MarketPlace.Team.UserId == userId),
            p => 
                p.Include(pr => pr.CatalogProduct.Catalog)
                    .Include(pr => pr.InventoryProductImages),
            cancellationToken: cancellationToken);
        if (product == null)
        {
            throw new CannotUpdateException(nameof(product));
        }

        product.Price = request.Price;
        product.Title = request.Title;
        product.Description = request.Description;
        product.Url = request.Url;
        var skuCode = request.SkuCode.Trim();
        if (skuCode != product.SkuCode)
        {
            var isExist =await 
                _context.InventoryProducts.AnyAsync(p => p.SkuCode == skuCode && (p.MarketPlace.Team.UserId == userId), cancellationToken: cancellationToken);
            if (isExist)
            {
                throw new AlreadyExistsException(nameof(request.SkuCode));
            }
            product.SkuCode = skuCode;
        }
        product.StockStatus = request.StockStatus.ToInt();
        product.Stock = request.Stock;
        _context.InventoryProducts.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
        return new UpdateInventoryProductResponseModel(product);
    }

}

public class UpdateInventoryProductResponseModel : InventoryProductDetailDto
{
    public UpdateInventoryProductResponseModel(InventoryProduct product) : base(product)
    {
    }
}