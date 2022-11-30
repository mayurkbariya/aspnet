using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.ProductListingImages.Commands.FixProductListingImageOrder;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.ProductListingImages.Commands.DeleteProductListingImage;

public class DeleteProductListingImageRequestModel : IRequest<DeleteProductListingImageResponseModel>
{
    public int Id { get; set; }
}

public class DeleteProductListingImageRequestModelValidator : AbstractValidator<DeleteProductListingImageRequestModel>
{
    public DeleteProductListingImageRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    DeleteProductListingImageRequestHandler : IRequestHandler<DeleteProductListingImageRequestModel,
        DeleteProductListingImageResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IImageService _imageService;
    private readonly IMediator _mediator;
    public DeleteProductListingImageRequestHandler(ApplicationDbContext context, ISessionService sessionService, IImageService imageService, IMediator mediator)
    {
        _context = context;
        _sessionService = sessionService;
        _imageService = imageService;
        _mediator = mediator;
    }

    public async Task<DeleteProductListingImageResponseModel> Handle(DeleteProductListingImageRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var image = await _context.ProductListingImages.GetByReadOnlyAsync(p =>
            p.Id == request.Id && p.ProductListing.MarketPlace.Team.UserId == userId, cancellationToken: cancellationToken);
        if (image == null)
        {
            throw new CannotDeleteException(nameof(image));
        }
        _context.ProductListingImages.Remove(image);
        await _context.SaveChangesAsync(cancellationToken);
        await _imageService.DeleteImage(image.Url);
        await _mediator.Send(new FixProductListingImageOrderRequestModel()
        {
            Id = image.ProductListingId
        }, cancellationToken);
        return new DeleteProductListingImageResponseModel();
    }
}

public class DeleteProductListingImageResponseModel
{

}