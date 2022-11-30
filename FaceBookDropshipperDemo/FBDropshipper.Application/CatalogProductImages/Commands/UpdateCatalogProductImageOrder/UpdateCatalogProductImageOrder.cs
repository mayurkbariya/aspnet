using FBDropshipper.Application.CatalogProductImages.Commands.FixCatalogProductImageOrder;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.CatalogProductImages.Commands.UpdateCatalogProductImageOrder;

public class UpdateCatalogProductImageOrderRequestModel : IRequest<UpdateCatalogProductImageOrderResponseModel>
{
    public int Id { get; set; }
    public int Order { get; set; }
}

public class
    UpdateCatalogProductImageOrderRequestModelValidator : AbstractValidator<UpdateCatalogProductImageOrderRequestModel>
{
    public UpdateCatalogProductImageOrderRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
        RuleFor(p => p.Order).Required();
    }
}

public class
    UpdateCatalogProductImageOrderRequestHandler : IRequestHandler<UpdateCatalogProductImageOrderRequestModel,
        UpdateCatalogProductImageOrderResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly ISessionService _sessionService;
    public UpdateCatalogProductImageOrderRequestHandler(ApplicationDbContext context, ISessionService sessionService, IMediator mediator)
    {
        _context = context;
        _sessionService = sessionService;
        _mediator = mediator;
    }

    public async Task<UpdateCatalogProductImageOrderResponseModel> Handle(
        UpdateCatalogProductImageOrderRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var image = await _context.CatalogProductImages.GetByReadOnlyAsync(p =>
            p.Id == request.Id && p.CatalogProduct.Catalog.UserId == userId, cancellationToken: cancellationToken);
        if (image == null)
        {
            throw new CannotDeleteException(nameof(image));
        }

        image.Order = request.Order;
        _context.CatalogProductImages.Update(image);
        await _context.SaveChangesAsync(cancellationToken);
        await _mediator.Send(new FixCatalogProductImageOrderRequestModel()
        {
            CatalogProductId = image.CatalogProductId
        }, cancellationToken);
        return new UpdateCatalogProductImageOrderResponseModel();
    }

}

public class UpdateCatalogProductImageOrderResponseModel
{

}