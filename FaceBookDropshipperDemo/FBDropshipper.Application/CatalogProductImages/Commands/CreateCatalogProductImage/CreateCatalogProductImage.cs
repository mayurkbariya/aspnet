using FBDropshipper.Application.CatalogProductImages.Models;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.CatalogProductImages.Commands.CreateCatalogProductImage;

public class CreateCatalogProductImageRequestModel : IRequest<CreateCatalogProductImageResponseModel>
{
    public int CatalogProductId { get; set; }
    public string Image { get; set; }
}

public class CreateCatalogProductImageRequestModelValidator : AbstractValidator<CreateCatalogProductImageRequestModel>
{
    public CreateCatalogProductImageRequestModelValidator()
    {
        RuleFor(p => p.CatalogProductId).Required();
        RuleFor(p => p.Image).Required();
    }
}

public class
    CreateCatalogProductImageRequestHandler : IRequestHandler<CreateCatalogProductImageRequestModel,
        CreateCatalogProductImageResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IImageService _imageService;
    public CreateCatalogProductImageRequestHandler(ApplicationDbContext context, ISessionService sessionService, IImageService imageService)
    {
        _context = context;
        _sessionService = sessionService;
        _imageService = imageService;
    }

    public async Task<CreateCatalogProductImageResponseModel> Handle(CreateCatalogProductImageRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var catalogProduct = await _context
            .CatalogProducts
            .ActiveAny(p => p.Id == request.CatalogProductId 
                            
                            && (p.Catalog.UserId == userId || p.Catalog.UserId == null));
        if (!catalogProduct)
        {
            throw new NotFoundException(nameof(catalogProduct));
        }

        var order = await _context.CatalogProductImages.Where(p => p.CatalogProductId == request.CatalogProductId)
            .Select(p => p.Order)
            .DefaultIfEmpty()
            .MaxAsync(cancellationToken);
        var image = new CatalogProductImage()
        {
            Url = await _imageService.SaveImage(request.Image),
            CatalogProductId = request.CatalogProductId,
            Order = order,
        };
        _context.CatalogProductImages.Add(image);
        await _context.SaveChangesAsync(cancellationToken);
        return new CreateCatalogProductImageResponseModel(image);
    }

}

public class CreateCatalogProductImageResponseModel : CatalogProductImageDto
{
    public CreateCatalogProductImageResponseModel(CatalogProductImage image) : base(image)
    {
    }
}