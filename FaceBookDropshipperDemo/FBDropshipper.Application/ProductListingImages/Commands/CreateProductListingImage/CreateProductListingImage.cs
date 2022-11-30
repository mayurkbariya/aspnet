using FBDropshipper.Application.ProductListingImages.Models;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.ProductListingImages.Commands.CreateProductListingImage;

public class CreateProductListingImageRequestModel : IRequest<CreateProductListingImageResponseModel>
{
    public int Id { get; set; }
    public string Image { get; set; }
}

public class CreateProductListingImageRequestModelValidator : AbstractValidator<CreateProductListingImageRequestModel>
{
    public CreateProductListingImageRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
        RuleFor(p => p.Image).Required();
    }
}

public class
    CreateProductListingImageRequestHandler : IRequestHandler<CreateProductListingImageRequestModel,
        CreateProductListingImageResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IImageService _imageService;
    public CreateProductListingImageRequestHandler(ApplicationDbContext context, ISessionService sessionService, IImageService imageService)
    {
        _context = context;
        _sessionService = sessionService;
        _imageService = imageService;
    }

    public async Task<CreateProductListingImageResponseModel> Handle(CreateProductListingImageRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var catalogProduct = await _context
            .InventoryProducts
            .ActiveAny(p => p.Id == request.Id && 
                            (p.MarketPlace.Team.UserId == userId));
        if (!catalogProduct)
        {
            throw new NotFoundException(nameof(catalogProduct));
        }
        var order = await _context.ProductListingImages.Where(p => p.ProductListingId == request.Id)
            .Select(p => p.Order)
            .DefaultIfEmpty()
            .MaxAsync(cancellationToken);
        var image = new ProductListingImage()
        {
            Url = await _imageService.SaveImage(request.Image),
            ProductListingId = request.Id,
            Order = order + 1
        };
        _context.ProductListingImages.Add(image);
        await _context.SaveChangesAsync(cancellationToken);
        return new CreateProductListingImageResponseModel(image);
    }

}

public class CreateProductListingImageResponseModel : ProductListingImageDto
{
    public CreateProductListingImageResponseModel(ProductListingImage image) : base(image)
    {
    }
}