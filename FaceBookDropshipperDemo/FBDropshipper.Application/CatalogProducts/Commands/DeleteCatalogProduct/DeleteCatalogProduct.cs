using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Images.Commands.DeleteImages;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.CatalogProducts.Commands.DeleteCatalogProduct;

public class DeleteCatalogProductRequestModel : IRequest<DeleteCatalogProductResponseModel>
{
    public int Id { get; set; }
}

public class DeleteCatalogProductRequestModelValidator : AbstractValidator<DeleteCatalogProductRequestModel>
{
    public DeleteCatalogProductRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    DeleteCatalogProductRequestHandler : IRequestHandler<DeleteCatalogProductRequestModel,
        DeleteCatalogProductResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IImageService _imageService;
    private readonly IBackgroundTaskQueueService _queueService;
    public DeleteCatalogProductRequestHandler(ApplicationDbContext context, ISessionService sessionService, IImageService imageService, IBackgroundTaskQueueService queueService)
    {
        _context = context;
        _sessionService = sessionService;
        _imageService = imageService;
        _queueService = queueService;
    }

    public async Task<DeleteCatalogProductResponseModel> Handle(DeleteCatalogProductRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var product = await _context.CatalogProducts.GetByReadOnlyAsync(p => 
            p.Id == request.Id && p.Catalog.UserId == userId, 
            p => 
                p.Include(pr => pr.CatalogProductImages)
                    .Include(pr => pr.InventoryProducts)
                    .ThenInclude(pr => pr.InventoryProductImages)
                    .Include(pr => pr.InventoryProducts)
                    .ThenInclude(pr => pr.ProductLists)
                    .ThenInclude(pr => pr.ProductListingImages)
                    .Include(pr => pr.InventoryProducts)
                    .ThenInclude(pr => pr.ProductLists)
                    .ThenInclude(pr => pr.Orders)
            ,
            cancellationToken: cancellationToken);
        if (product == null)
        {
            throw new CannotDeleteException(nameof(product));
        }

        var urls = new List<string>();
        product.CatalogProductImages.ToList().ForEach(p =>
        {
            _context.CatalogProductImages.Remove(p);
            urls.Add(p.Url);
        });
        product.InventoryProducts.ToList().ForEach(i =>
        {
            i.ProductLists.ToList().ForEach(p =>
            {
                _context.ProductListings.Remove(p);
                p.ProductListingImages.ToList().ForEach(pr =>
                {
                    _context.ProductListingImages.Remove(pr);
                    urls.Add(pr.Url);
                });
                _context.Orders.RemoveRange(p.Orders);
            });
        });
        _context.CatalogProducts.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
        _queueService.QueueBackgroundWorkItem(new DeleteImagesRequestModel()
        {
            Urls = urls.ToArray()
        });
        return new DeleteCatalogProductResponseModel();
    }

}

public class DeleteCatalogProductResponseModel
{

}