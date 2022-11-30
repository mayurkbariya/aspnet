using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Images.Commands.DeleteImages;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.ProductListings.Commands.DeleteProductListingBulk;

public class DeleteProductListingBulkRequestModel : IRequest<DeleteProductListingBulkResponseModel>
{
    public int[] Ids { get; set; }
}

public class DeleteProductListingBulkRequestModelValidator : AbstractValidator<DeleteProductListingBulkRequestModel>
{
    public DeleteProductListingBulkRequestModelValidator()
    {
        RuleFor(p => p.Ids).Required().Max(50);
    }
}

public class
    DeleteProductListingBulkRequestHandler : IRequestHandler<DeleteProductListingBulkRequestModel,
        DeleteProductListingBulkResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IBackgroundTaskQueueService _queueService;
        public DeleteProductListingBulkRequestHandler(ApplicationDbContext context, ISessionService sessionService, IBackgroundTaskQueueService queueService)
    {
        _context = context;
        _sessionService = sessionService;
        _queueService = queueService;
    }

    public async Task<DeleteProductListingBulkResponseModel> Handle(DeleteProductListingBulkRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var products = await _context.ProductListings.Where(p =>
                request.Ids.Contains(p.Id) && (p.MarketPlace.Team.UserId == userId))
                .Include(pr => pr.Orders)
                .Include(pr => pr.ProductListingImages)
                .ToListAsync(cancellationToken: cancellationToken);
        

        var urls = new List<string>();
        foreach (var product in products)
        {
            product.ProductListingImages.ToList().ForEach(p =>
            {
                _context.ProductListingImages.Remove(p);
                urls.Add(p.Url);
            });
            _context.Orders.RemoveRange(product.Orders);
            _context.ProductListings.Remove(product);
        }
        await _context.SaveChangesAsync(cancellationToken);
        _queueService.QueueBackgroundWorkItem(new DeleteImagesRequestModel()
        {
            Urls = urls.ToArray()
        });
        return new DeleteProductListingBulkResponseModel()
        {
            Count = products.Count
        };
    }

}

public class DeleteProductListingBulkResponseModel
{
    public int Count { get; set; }
}