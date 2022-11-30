using FBDropshipper.Application.Catalogs.Models;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Catalogs.Queries.GetCatalogById;

public class GetCatalogByIdRequestModel : IRequest<GetCatalogByIdResponseModel>
{
    public int Id { get; set; }
}

public class GetCatalogByIdRequestModelValidator : AbstractValidator<GetCatalogByIdRequestModel>
{
    public GetCatalogByIdRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    GetCatalogByIdRequestHandler : IRequestHandler<GetCatalogByIdRequestModel, GetCatalogByIdResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetCatalogByIdRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetCatalogByIdResponseModel> Handle(GetCatalogByIdRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var catalog = await _context.Catalogs.GetByWithSelectAsync(p => 
            p.Id == request.Id && p.MarketPlace.Team.UserId == userId,
            CatalogSelector.Selector, cancellationToken: cancellationToken);
        if (catalog == null)
        {
            throw new NotFoundException(nameof(catalog));
        }
        return catalog.CreateCopy<GetCatalogByIdResponseModel>();
    }

}

public class GetCatalogByIdResponseModel : CatalogDto
{

}