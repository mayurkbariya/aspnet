using FBDropshipper.Application.ProductListingImages.Commands.FixProductListingImageOrder;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.ProductListingImages.Commands.UpdateProductListingImageOrder;

public class UpdateProductListingImageOrderRequestModel : IRequest<UpdateProductListingImageOrderResponseModel>
{
    public int Id { get; set; }
    public int Order { get; set; }
}

public class
    UpdateProductListingImageOrderRequestModelValidator : AbstractValidator<
        UpdateProductListingImageOrderRequestModel>
{
    public UpdateProductListingImageOrderRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
        RuleFor(p => p.Order).Required();
    }
}

public class
    UpdateProductListingImageOrderRequestHandler : IRequestHandler<UpdateProductListingImageOrderRequestModel,
        UpdateProductListingImageOrderResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly ISessionService _sessionService;

    public UpdateProductListingImageOrderRequestHandler(ApplicationDbContext context, ISessionService sessionService,
        IMediator mediator)
    {
        _context = context;
        _sessionService = sessionService;
        _mediator = mediator;
    }

    public async Task<UpdateProductListingImageOrderResponseModel> Handle(
        UpdateProductListingImageOrderRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetUserId();
        var image = await _context.ProductListingImages.GetByReadOnlyAsync(p =>
            p.Id == request.Id && p.ProductListing.MarketPlace.Team.UserId == userId, cancellationToken: cancellationToken);
        if (image == null)
        {
            throw new CannotDeleteException(nameof(image));
        }
        image.Order = request.Order;
        _context.ProductListingImages.Update(image);
        await _context.SaveChangesAsync(cancellationToken);
        await _mediator.Send(new FixProductListingImageOrderRequestModel()
        {
            Id = image.ProductListingId
        }, cancellationToken);
        return new UpdateProductListingImageOrderResponseModel();
    }
}

public class UpdateProductListingImageOrderResponseModel
{
}