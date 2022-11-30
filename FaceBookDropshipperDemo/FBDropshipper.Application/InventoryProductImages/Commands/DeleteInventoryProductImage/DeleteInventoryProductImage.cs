using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.InventoryProductImages.Commands.FixInventoryProductImageOrder;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.InventoryProductImages.Commands.DeleteInventoryProductImage;

public class DeleteInventoryProductImageRequestModel : IRequest<DeleteInventoryProductImageResponseModel>
{
    public int Id { get; set; }
}

public class DeleteInventoryProductImageRequestModelValidator : AbstractValidator<DeleteInventoryProductImageRequestModel>
{
    public DeleteInventoryProductImageRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    DeleteInventoryProductImageRequestHandler : IRequestHandler<DeleteInventoryProductImageRequestModel,
        DeleteInventoryProductImageResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IImageService _imageService;
    private readonly IMediator _mediator;
    public DeleteInventoryProductImageRequestHandler(ApplicationDbContext context, ISessionService sessionService, IImageService imageService, IMediator mediator)
    {
        _context = context;
        _sessionService = sessionService;
        _imageService = imageService;
        _mediator = mediator;
    }

    public async Task<DeleteInventoryProductImageResponseModel> Handle(DeleteInventoryProductImageRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var image = await _context.InventoryProductImages.GetByReadOnlyAsync(p =>
            p.Id == request.Id && p.InventoryProduct.MarketPlace.Team.UserId == userId, cancellationToken: cancellationToken);
        if (image == null)
        {
            throw new CannotDeleteException(nameof(image));
        }
        _context.InventoryProductImages.Remove(image);
        await _context.SaveChangesAsync(cancellationToken);
        await _imageService.DeleteImage(image.Url);
        await _mediator.Send(new FixInventoryProductImageOrderRequestModel()
        {
            InventoryProductId = image.InventoryProductId
        }, cancellationToken);
        return new DeleteInventoryProductImageResponseModel();
    }
}

public class DeleteInventoryProductImageResponseModel
{

}