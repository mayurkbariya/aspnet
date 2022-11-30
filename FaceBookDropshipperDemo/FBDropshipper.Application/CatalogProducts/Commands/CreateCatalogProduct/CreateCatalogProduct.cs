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

namespace FBDropshipper.Application.CatalogProducts.Commands.CreateCatalogProduct;

public class CreateCatalogProductRequestModel : IRequest<CreateCatalogProductResponseModel>
{
    public int CatalogId { get; set; }
    public string Title { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public string SkuCode { get; set; }
    public string Url { get; set; }
    public StockStatus StockStatus { get; set; }
    public int Stock { get; set; }
}

public class CreateCatalogProductRequestModelValidator : AbstractValidator<CreateCatalogProductRequestModel>
{
    public CreateCatalogProductRequestModelValidator()
    {
        RuleFor(p => p.Title).Required().Max(255);
        RuleFor(p => p.Description).Required();
        RuleFor(p => p.SkuCode).Required().Max(255);
        RuleFor(p => p.StockStatus).IsInEnum();
        RuleFor(p => p.Price).Required();
        RuleFor(p => p.Stock).Min(0);
    }
}

public class
    CreateCatalogProductRequestHandler : IRequestHandler<CreateCatalogProductRequestModel,
        CreateCatalogProductResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public CreateCatalogProductRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<CreateCatalogProductResponseModel> Handle(CreateCatalogProductRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var catalog = await _context.Catalogs
            .GetByReadOnlyAsync(p => p.Id == request.CatalogId && p.UserId == userId,
                cancellationToken: cancellationToken);
        if (catalog == null)
        {
            throw new NotFoundException(nameof(catalog));
        }

        var skuCode = request.SkuCode.Trim();
        var isExist =await 
            _context.CatalogProducts.AnyAsync(p => p.SkuCode == skuCode && p.CatalogId == request.CatalogId, cancellationToken: cancellationToken);
        if (isExist)
        {
            throw new AlreadyExistsException(nameof(request.SkuCode));
        }
        var product = new CatalogProduct()
        {
            Description = request.Description,
            Stock = request.Stock,
            Title = request.Title,
            Price = request.Price,
            CatalogId = request.CatalogId,
            Url = request.Url,
            StockStatus = request.StockStatus.ToInt(),
            SkuCode = request.SkuCode,
        };
        _context.CatalogProducts.Add(product);
        await _context.SaveChangesAsync(cancellationToken);
        product.Catalog = catalog;
        return new CreateCatalogProductResponseModel(product);
    }

}

public class CreateCatalogProductResponseModel : CatalogProductDto
{
    public CreateCatalogProductResponseModel(CatalogProduct product) : base(product)
    {
    }
}