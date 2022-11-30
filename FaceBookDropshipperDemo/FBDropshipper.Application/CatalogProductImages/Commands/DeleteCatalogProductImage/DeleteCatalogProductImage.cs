using FBDropshipper.Application.CatalogProductImages.Commands.FixCatalogProductImageOrder;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.CatalogProductImages.Commands.DeleteCatalogProductImage;

public class DeleteCatalogProductImageRequestModel : IRequest<DeleteCatalogProductImageResponseModel>
{
    public int Id { get; set; }
}

public class DeleteCatalogProductImageRequestModelValidator : AbstractValidator<DeleteCatalogProductImageRequestModel>
{
    public DeleteCatalogProductImageRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    DeleteCatalogProductImageRequestHandler : IRequestHandler<DeleteCatalogProductImageRequestModel,
        DeleteCatalogProductImageResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IImageService _imageService;
    private readonly IMediator _mediator;
    public DeleteCatalogProductImageRequestHandler(ApplicationDbContext context, ISessionService sessionService, IImageService imageService, IMediator mediator)
    {
        _context = context;
        _sessionService = sessionService;
        _imageService = imageService;
        _mediator = mediator;
    }

    public async Task<DeleteCatalogProductImageResponseModel> Handle(DeleteCatalogProductImageRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var image = await _context.CatalogProductImages.GetByReadOnlyAsync(p =>
            p.Id == request.Id && p.CatalogProduct.Catalog.UserId == userId, cancellationToken: cancellationToken);
        if (image == null)
        {
            throw new CannotDeleteException(nameof(image));
        }
        _context.CatalogProductImages.Remove(image);
        await _context.SaveChangesAsync(cancellationToken);
        await _imageService.DeleteImage(image.Url);
        await _mediator.Send(new FixCatalogProductImageOrderRequestModel()
        {
            CatalogProductId = image.CatalogProductId
        }, cancellationToken);
        return new DeleteCatalogProductImageResponseModel();
    }
}

public class DeleteCatalogProductImageResponseModel
{

}