using FBDropshipper.Application.InventoryProductImages.Models;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.InventoryProductImages.Commands.CreateInventoryProductImage;

public class CreateInventoryProductImageRequestModel : IRequest<CreateInventoryProductImageResponseModel>
{
    public int InventoryProductId { get; set; }
    public string Image { get; set; }
}

public class CreateInventoryProductImageRequestModelValidator : AbstractValidator<CreateInventoryProductImageRequestModel>
{
    public CreateInventoryProductImageRequestModelValidator()
    {
        RuleFor(p => p.InventoryProductId).Required();
        RuleFor(p => p.Image).Required();
    }
}

public class
    CreateInventoryProductImageRequestHandler : IRequestHandler<CreateInventoryProductImageRequestModel,
        CreateInventoryProductImageResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IImageService _imageService;
    public CreateInventoryProductImageRequestHandler(ApplicationDbContext context, ISessionService sessionService, IImageService imageService)
    {
        _context = context;
        _sessionService = sessionService;
        _imageService = imageService;
    }

    public async Task<CreateInventoryProductImageResponseModel> Handle(CreateInventoryProductImageRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var catalogProduct = await _context
            .InventoryProducts
            .ActiveAny(p => p.Id == request.InventoryProductId && 
                            (p.CatalogProduct.Catalog.UserId == userId || p.CatalogProduct.Catalog.UserId == null));
        if (!catalogProduct)
        {
            throw new NotFoundException(nameof(catalogProduct));
        }
        var order = await _context.InventoryProductImages.Where(p => p.InventoryProductId == request.InventoryProductId)
            .Select(p => p.Order)
            .DefaultIfEmpty()
            .MaxAsync(cancellationToken);
        var image = new InventoryProductImage()
        {
            Url = await _imageService.SaveImage(request.Image),
            InventoryProductId = request.InventoryProductId,
            Order = order + 1
        };
        _context.InventoryProductImages.Add(image);
        await _context.SaveChangesAsync(cancellationToken);
        return new CreateInventoryProductImageResponseModel(image);
    }

}

public class CreateInventoryProductImageResponseModel : InventoryProductImageDto
{
    public CreateInventoryProductImageResponseModel(InventoryProductImage image) : base(image)
    {
    }
}