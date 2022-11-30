using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Images.Commands.DeleteImages;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.InventoryProducts.Commands.DeleteInventoryProductBulk;

public class DeleteInventoryProductBulkRequestModel : IRequest<DeleteInventoryProductBulkResponseModel>
{
    public int[] Ids { get; set; }
}

public class DeleteInventoryProductBulkRequestModelValidator : AbstractValidator<DeleteInventoryProductBulkRequestModel>
{
    public DeleteInventoryProductBulkRequestModelValidator()
    {
        RuleFor(p => p.Ids).Required().Max(50);
    }
}

public class
    DeleteInventoryProductBulkRequestHandler : IRequestHandler<DeleteInventoryProductBulkRequestModel,
        DeleteInventoryProductBulkResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IBackgroundTaskQueueService _queueService;
    public DeleteInventoryProductBulkRequestHandler(ApplicationDbContext context, ISessionService sessionService, IBackgroundTaskQueueService queueService)
    {
        _context = context;
        _sessionService = sessionService;
        _queueService = queueService;
    }

    public async Task<DeleteInventoryProductBulkResponseModel> Handle(DeleteInventoryProductBulkRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetUserId();
        var products = await _context.InventoryProducts.Where(p =>
                request.Ids.Contains(p.Id) && (p.MarketPlace.Team.UserId == userId))
                .Include(pr => pr.InventoryProductImages)
                    .Include(pr => pr.ProductLists)
                    .ThenInclude(pr => pr.ProductListingImages)
                    .Include(pr => pr.ProductLists)
                    .ThenInclude(pr => pr.Orders)
                .ToListAsync(cancellationToken);
        

        var urls = new List<string>();
        foreach (var product in products)
        {
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
        }
        
        await _context.SaveChangesAsync(cancellationToken);
        _queueService.QueueBackgroundWorkItem(new DeleteImagesRequestModel()
        {
            Urls = urls.ToArray()
        });
        return new DeleteInventoryProductBulkResponseModel()
        {
            Count = products.Count
        };
    }

}

public class DeleteInventoryProductBulkResponseModel
{
    public int Count { get; set; }
}