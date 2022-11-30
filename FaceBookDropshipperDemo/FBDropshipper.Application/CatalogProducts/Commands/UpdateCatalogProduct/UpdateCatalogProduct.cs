using FBDropshipper.Application.CatalogProducts.Models;
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

namespace FBDropshipper.Application.CatalogProducts.Commands.UpdateCatalogProduct;

public class UpdateCatalogProductRequestModel : IRequest<UpdateCatalogProductResponseModel>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string SkuCode { get; set; }
    public string Url { get; set; }
    public StockStatus StockStatus { get; set; }
    public int Stock { get; set; }
}

public class UpdateCatalogProductRequestModelValidator : AbstractValidator<UpdateCatalogProductRequestModel>
{
    public UpdateCatalogProductRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
        RuleFor(p => p.Title).Required().Max(255);
        RuleFor(p => p.Price).Required();
        RuleFor(p => p.Description).Required();
        RuleFor(p => p.SkuCode).Required().Max(255);
        RuleFor(p => p.StockStatus).IsInEnum();
        RuleFor(p => p.Stock).Min(0);
    }
}

public class
    UpdateCatalogProductRequestHandler : IRequestHandler<UpdateCatalogProductRequestModel,
        UpdateCatalogProductResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public UpdateCatalogProductRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<UpdateCatalogProductResponseModel> Handle(UpdateCatalogProductRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var product = await _context.CatalogProducts.GetByReadOnlyAsync(p => 
            p.Id == request.Id && p.Catalog.UserId == userId,
            p => 
                p.Include(pr => pr.Catalog)
                    .Include(pr => pr.CatalogProductImages),
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
                _context.CatalogProducts.AnyAsync(p => p.SkuCode == skuCode && p.CatalogId == product.CatalogId, cancellationToken: cancellationToken);
            if (isExist)
            {
                throw new AlreadyExistsException(nameof(request.SkuCode));
            }
            product.SkuCode = skuCode;
        }
        product.StockStatus = request.StockStatus.ToInt();
        product.Stock = request.Stock;
        _context.CatalogProducts.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
        return new UpdateCatalogProductResponseModel(product);
    }

}

public class UpdateCatalogProductResponseModel : CatalogProductDetailDto
{
    public UpdateCatalogProductResponseModel(CatalogProduct product) : base(product)
    {
    }
}