using FBDropshipper.Application.Catalogs.Models;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.Catalogs.Commands.UpdateCatalog;

public class UpdateCatalogRequestModel : IRequest<UpdateCatalogResponseModel>
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class UpdateCatalogRequestModelValidator : AbstractValidator<UpdateCatalogRequestModel>
{
    public UpdateCatalogRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
        RuleFor(p => p.Name).Required().Max(255);
    }
}

public class
    UpdateCatalogRequestHandler : IRequestHandler<UpdateCatalogRequestModel, UpdateCatalogResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public UpdateCatalogRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<UpdateCatalogResponseModel> Handle(UpdateCatalogRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var catalog = await _context.Catalogs.GetByReadOnlyAsync(p => p.Id == request.Id
                                                                      && p.UserId == userId,
            p => p.Include(pr => pr.MarketPlace),
            cancellationToken: cancellationToken);
        if (catalog == null)
        {
            throw new NotFoundException(nameof(catalog));
        }
        catalog.Name = request.Name;
        _context.Catalogs.Update(catalog);
        await _context.SaveChangesAsync(cancellationToken);
        return new UpdateCatalogResponseModel(catalog);
    }

}

public class UpdateCatalogResponseModel : CatalogDto
{
    public UpdateCatalogResponseModel(Catalog catalog) : base(catalog)
    {
    }
}