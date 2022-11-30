using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Images.Commands.DeleteImages;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.ProductListings.Commands.DeleteProductListing;

public class DeleteProductListingRequestModel : IRequest<DeleteProductListingResponseModel>
{
    public int Id { get; set; }

}

public class DeleteProductListingRequestModelValidator : AbstractValidator<DeleteProductListingRequestModel>
{
    public DeleteProductListingRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    DeleteProductListingRequestHandler : IRequestHandler<DeleteProductListingRequestModel,
        DeleteProductListingResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IBackgroundTaskQueueService _queueService;
    public DeleteProductListingRequestHandler(ApplicationDbContext context, ISessionService sessionService, IBackgroundTaskQueueService queueService)
    {
        _context = context;
        _sessionService = sessionService;
        _queueService = queueService;
    }

    public async Task<DeleteProductListingResponseModel> Handle(DeleteProductListingRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var product = await _context.ProductListings.GetByReadOnlyAsync(p =>
                p.Id == request.Id && (p.MarketPlace.Team.UserId == userId),
            p => p
                .Include(pr => pr.Orders)
                .Include(pr => pr.ProductListingImages),
            cancellationToken);
        if (product == null)
        {
            throw new CannotDeleteException(nameof(product));
        }

        var urls = new List<string>();
        product.ProductListingImages.ToList().ForEach(p =>
        {
            _context.ProductListingImages.Remove(p);
            urls.Add(p.Url);
        });
        _context.Orders.RemoveRange(product.Orders);
        _context.ProductListings.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
        _queueService.QueueBackgroundWorkItem(new DeleteImagesRequestModel()
        {
            Urls = urls.ToArray()
        });
        return new DeleteProductListingResponseModel();
    }

}

public class DeleteProductListingResponseModel
{

}