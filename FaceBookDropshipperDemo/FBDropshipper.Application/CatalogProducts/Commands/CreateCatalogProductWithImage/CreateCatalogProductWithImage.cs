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
using Microsoft.Extensions.Logging;

namespace FBDropshipper.Application.CatalogProducts.Commands.CreateCatalogProductWithImage;

public class CreateCatalogProductWithImageRequestModel : IRequest<CreateCatalogProductWithImageResponseModel>
{
    public int CatalogId { get; set; }
    public string Title { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public string SkuCode { get; set; }
    public string Url { get; set; }
    public StockStatus StockStatus { get; set; }
    public int Stock { get; set; }
    public string[] Images { get; set; }
}

public class
    CreateCatalogProductWithImageRequestModelValidator : AbstractValidator<CreateCatalogProductWithImageRequestModel>
{
    public CreateCatalogProductWithImageRequestModelValidator()
    {
        RuleFor(p => p.Title).Required().Max(255);
        RuleFor(p => p.Description).Required();
        RuleFor(p => p.SkuCode).Required().Max(255);
        RuleFor(p => p.StockStatus).IsInEnum();
        RuleFor(p => p.Price).Required();
        RuleFor(p => p.Stock).Min(0);
        RuleFor(p => p.Images).Required().Max();
        RuleForEach(p => p.Images).Required();
    }
}

public class
    CreateCatalogProductWithImageRequestHandler : IRequestHandler<CreateCatalogProductWithImageRequestModel,
        CreateCatalogProductWithImageResponseModel>
{
    private readonly ILogger<CreateCatalogProductWithImageRequestHandler> _logger;
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IImageService _imageService;
    public CreateCatalogProductWithImageRequestHandler(ApplicationDbContext context, ISessionService sessionService, IImageService imageService, ILogger<CreateCatalogProductWithImageRequestHandler> logger)
    {
        _context = context;
        _sessionService = sessionService;
        _imageService = imageService;
        _logger = logger;
    }

    public async Task<CreateCatalogProductWithImageResponseModel> Handle(
        CreateCatalogProductWithImageRequestModel request,
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

        var list = new List<CatalogProductImage>();
        int order = 1;
        try
        {
            foreach (var image in request.Images)
            {
                var item = new CatalogProductImage()
                {
                    Url = await _imageService.SaveImage(image),
                    Order = order++,
                };         
                list.Add(item);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, null);
            foreach (var image in list)
            {
                await _imageService.DeleteImage(image.Url);
            }
            throw new BadRequestException("Invalid Image : " + order);
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
            CatalogProductImages = list
        };
        _context.CatalogProducts.Add(product);
        await _context.SaveChangesAsync(cancellationToken);
        product.Catalog = catalog;
        return new CreateCatalogProductWithImageResponseModel(product);
    }

}

public class CreateCatalogProductWithImageResponseModel : CatalogProductDto
{
    public CreateCatalogProductWithImageResponseModel(CatalogProduct product) : base(product)
    {
    }
}