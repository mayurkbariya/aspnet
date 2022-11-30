using System.Linq.Expressions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.ListingTemplates.Models;
using FBDropshipper.Application.Shared;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.ListingTemplates.Queries.GetListingTemplates;

public class GetListingTemplatesRequestModel : GetPagedRequest<GetListingTemplatesResponseModel>
{
    public int MarketPlaceId { get; set; }
}

public class GetListingTemplatesRequestModelValidator : PageRequestValidator<GetListingTemplatesRequestModel>
{
    public GetListingTemplatesRequestModelValidator()
    {
        RuleFor(p => p.MarketPlaceId).Required();
    }
}

public class
    GetListingTemplatesRequestHandler : IRequestHandler<GetListingTemplatesRequestModel,
        GetListingTemplatesResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetListingTemplatesRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetListingTemplatesResponseModel> Handle(GetListingTemplatesRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        Expression<Func<ListingTemplate, bool>> query = p =>
            p.MarketPlaceId == request.MarketPlaceId && p.MarketPlace.Team.UserId == userId;
        if (request.Search.IsNotNullOrWhiteSpace())
        {
            query = query.AndAlso(p => p.Name.ToLower().Contains(request.Search));
        }

        var list = await _context.ListingTemplates.GetManyReadOnly(query, request)
            .Select(ListingTemplateSelector.Selector)
            .ToListAsync(cancellationToken);
        var count = await _context.ListingTemplates.ActiveCount(query, cancellationToken);
        return new GetListingTemplatesResponseModel()
        {
            Data = list,
            Count = count
        };
    }

}

public class GetListingTemplatesResponseModel
{
    public List<ListingTemplateDto> Data { get; set; }
    public int Count { get; set; }
}