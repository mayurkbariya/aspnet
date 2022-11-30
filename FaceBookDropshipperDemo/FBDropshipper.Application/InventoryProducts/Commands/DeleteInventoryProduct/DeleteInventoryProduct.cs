using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Images.Commands.DeleteImages;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.InventoryProducts.Commands.DeleteInventoryProduct;

public class DeleteInventoryProductRequestModel : IRequest<DeleteInventoryProductResponseModel>
{
    public int Id { get; set; }
}

public class DeleteInventoryProductRequestModelValidator : AbstractValidator<DeleteInventoryProductRequestModel>
{
    public DeleteInventoryProductRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    DeleteInventoryProductRequestHandler : IRequestHandler<DeleteInventoryProductRequestModel,
        DeleteInventoryProductResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IImageService _imageService;
    private readonly IBackgroundTaskQueueService _queueService;
    public DeleteInventoryProductRequestHandler(ApplicationDbContext context, ISessionService sessionService, IImageService imageService, IBackgroundTaskQueueService queueService)
    {
        _context = context;
        _sessionService = sessionService;
        _imageService = imageService;
        _queueService = queueService;
    }

    public async Task<DeleteInventoryProductResponseModel> Handle(DeleteInventoryProductRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var product = await _context.InventoryProducts.GetByReadOnlyAsync(p =>
                p.Id == request.Id && (p.CatalogProduct.Catalog.UserId == userId || p.CatalogProduct.Catalog.UserId == null),
            p => 
                p.Include(pr => pr.InventoryProductImages)
                    .Include(pr => pr.ProductLists)
                    .ThenInclude(pr => pr.ProductListingImages)
                    .Include(pr => pr.ProductLists)
                    .ThenInclude(pr => pr.Orders)
            ,
            cancellationToken);
        if (product == null)
        {
            throw new CannotDeleteException(nameof(product));
        }

        var urls = new List<string>();
        product.InventoryProductImages.ToList().ForEach(p =>
        {
            _context.InventoryProductImages.Remove(p);
            urls.Add(p.Url);
        });
        product.ProductLists.ToList().ForEach(p =>
        {
            _context.ProductListings.Remove(p);
            p.ProductListingImages.ToList().ForEach(pr =>
            {
                _context.ProductListingImages.Remove(pr);
                urls.Add(pr.Url);
            });
            _context.Orders.RemoveRange(p.Orders);
        });
        _context.InventoryProducts.Remove(product);
        
        await _context.SaveChangesAsync(cancellationToken);
        _queueService.QueueBackgroundWorkItem(new DeleteImagesRequestModel()
        {
            Urls = urls.ToArray()
        });
        return new DeleteInventoryProductResponseModel();
    }

}

public class DeleteInventoryProductResponseModel
{

}