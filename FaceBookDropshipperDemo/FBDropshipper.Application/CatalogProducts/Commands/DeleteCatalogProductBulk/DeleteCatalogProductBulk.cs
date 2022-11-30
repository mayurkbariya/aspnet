using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Images.Commands.DeleteImages;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.CatalogProducts.Commands.DeleteCatalogProductBulk;

public class DeleteCatalogProductBulkRequestModel : IRequest<DeleteCatalogProductBulkResponseModel>
{
    public int[] Ids { get; set; }
}

public class DeleteCatalogProductBulkRequestModelValidator : AbstractValidator<DeleteCatalogProductBulkRequestModel>
{
    public DeleteCatalogProductBulkRequestModelValidator()
    {
        RuleFor(p => p.Ids).Required().Max(50);
    }
}

public class
    DeleteCatalogProductBulkRequestHandler : IRequestHandler<DeleteCatalogProductBulkRequestModel,
        DeleteCatalogProductBulkResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IBackgroundTaskQueueService _queueService;
    public DeleteCatalogProductBulkRequestHandler(ApplicationDbContext context, IBackgroundTaskQueueService queueService, ISessionService sessionService)
    {
        _context = context;
        _queueService = queueService;
        _sessionService = sessionService;
    }

    public async Task<DeleteCatalogProductBulkResponseModel> Handle(DeleteCatalogProductBulkRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var products = await _context.CatalogProducts.Where(p => 
                request.Ids.Contains(p.Id) && p.Catalog.UserId == userId)
                .Include(pr => pr.CatalogProductImages)
                    .Include(pr => pr.InventoryProducts)
                    .ThenInclude(pr => pr.InventoryProductImages)
                    .Include(pr => pr.InventoryProducts)
                    .ThenInclude(pr => pr.ProductLists)
                    .ThenInclude(pr => pr.ProductListingImages)
                    .Include(pr => pr.InventoryProducts)
                    .ThenInclude(pr => pr.ProductLists)
                    .ThenInclude(pr => pr.Orders)
                .ToListAsync(cancellationToken: cancellationToken);

        var urls = new List<string>();
        foreach (var product in products)
        {
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
        }
        await _context.SaveChangesAsync(cancellationToken);
        _queueService.QueueBackgroundWorkItem(new DeleteImagesRequestModel()
        {
            Urls = urls.ToArray()
        });
        return new DeleteCatalogProductBulkResponseModel()
        {
            Count = products.Count
        };
    }

}

public class DeleteCatalogProductBulkResponseModel
{
    public int Count { get; set; }
}