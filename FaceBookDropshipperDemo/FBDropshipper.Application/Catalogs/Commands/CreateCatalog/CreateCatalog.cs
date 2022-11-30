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

namespace FBDropshipper.Application.Catalogs.Commands.CreateCatalog;

public class CreateCatalogRequestModel : IRequest<CreateCatalogResponseModel>
{
    public int MarketPlaceId { get; set; }
    public string Name { get; set; }
}

public class CreateCatalogRequestModelValidator : AbstractValidator<CreateCatalogRequestModel>
{
    public CreateCatalogRequestModelValidator()
    {
        RuleFor(p => p.MarketPlaceId).Required();
        RuleFor(p => p.Name).Required().Max(255);
    }
}

public class
    CreateCatalogRequestHandler : IRequestHandler<CreateCatalogRequestModel, CreateCatalogResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public CreateCatalogRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<CreateCatalogResponseModel> Handle(CreateCatalogRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var marketplace = await 
            _context.MarketPlaces.GetByReadOnlyAsync(p => p.Id == request.MarketPlaceId && p.Team.UserId == userId, cancellationToken: cancellationToken);
        if (marketplace == null)
        {
            throw new NotFoundException(nameof(marketplace));
        }
        var catalog = new Catalog()
        {
            Name = request.Name,
            CatalogType = CatalogType.NonIntegrated.ToInt(),
            UserId = userId,
            CanBeDeleted = true,
            MarketPlaceId = request.MarketPlaceId
        };
        _context.Catalogs.Add(catalog);
        await _context.SaveChangesAsync(cancellationToken);
        catalog.MarketPlace = marketplace;
        return new CreateCatalogResponseModel(catalog);
    }

}

public class CreateCatalogResponseModel : CatalogDto
{
    public CreateCatalogResponseModel(Catalog catalog) : base(catalog)
    {
        
    }
}