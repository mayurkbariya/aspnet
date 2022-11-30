using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Catalogs.Commands.DeleteCatalog;

public class DeleteCatalogRequestModel : IRequest<DeleteCatalogResponseModel>
{
    public int Id { get; set; }
}

public class DeleteCatalogRequestModelValidator : AbstractValidator<DeleteCatalogRequestModel>
{
    public DeleteCatalogRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    DeleteCatalogRequestHandler : IRequestHandler<DeleteCatalogRequestModel, DeleteCatalogResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public DeleteCatalogRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<DeleteCatalogResponseModel> Handle(DeleteCatalogRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var catalog = await _context.Catalogs.GetByReadOnlyAsync(p => p.Id == request.Id 
                                                                      && !p.CatalogProducts.Any()
                                                                      && p.UserId == userId, cancellationToken: cancellationToken);
        if (catalog == null)
        {
            throw new CannotDeleteException(nameof(catalog));
        }

        _context.Catalogs.Remove(catalog);
        await _context.SaveChangesAsync(cancellationToken);
        return new DeleteCatalogResponseModel();
    }

}

public class DeleteCatalogResponseModel
{

}