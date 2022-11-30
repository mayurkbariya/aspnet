using FBDropshipper.Application.InventoryProductImages.Commands.FixInventoryProductImageOrder;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.InventoryProductImages.Commands.UpdateInventoryProductImageOrder;

public class UpdateInventoryProductImageOrderRequestModel : IRequest<UpdateInventoryProductImageOrderResponseModel>
{
    public int Id { get; set; }
    public int Order { get; set; }
}

public class
    UpdateInventoryProductImageOrderRequestModelValidator : AbstractValidator<
        UpdateInventoryProductImageOrderRequestModel>
{
    public UpdateInventoryProductImageOrderRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
        RuleFor(p => p.Order).Required();
    }
}

public class
    UpdateInventoryProductImageOrderRequestHandler : IRequestHandler<UpdateInventoryProductImageOrderRequestModel,
        UpdateInventoryProductImageOrderResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly ISessionService _sessionService;

    public UpdateInventoryProductImageOrderRequestHandler(ApplicationDbContext context, ISessionService sessionService,
        IMediator mediator)
    {
        _context = context;
        _sessionService = sessionService;
        _mediator = mediator;
    }

    public async Task<UpdateInventoryProductImageOrderResponseModel> Handle(
        UpdateInventoryProductImageOrderRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var image = await _context.InventoryProductImages.GetByReadOnlyAsync(p =>
            p.Id == request.Id && p.InventoryProduct.CatalogProduct.Catalog.UserId == userId, cancellationToken: cancellationToken);
        if (image == null)
        {
            throw new CannotDeleteException(nameof(image));
        }
        image.Order = request.Order;
        _context.InventoryProductImages.Update(image);
        await _context.SaveChangesAsync(cancellationToken);
        await _mediator.Send(new FixInventoryProductImageOrderRequestModel()
        {
            InventoryProductId = image.InventoryProductId
        }, cancellationToken);
        return new UpdateInventoryProductImageOrderResponseModel();
    }
}

public class UpdateInventoryProductImageOrderResponseModel
{
}