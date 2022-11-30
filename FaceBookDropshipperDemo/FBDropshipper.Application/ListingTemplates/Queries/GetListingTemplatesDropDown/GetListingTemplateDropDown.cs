using System.Linq.Expressions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.ListingTemplates.Models;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.ListingTemplates.Queries.GetListingTemplatesDropDown;

public class GetListingTemplateDropDownRequestModel : IRequest<GetListingTemplateDropDownResponseModel>
{
    public int MarketPlaceId { get; set; }
}

public class GetListingTemplateDropDownRequestModelValidator : AbstractValidator<GetListingTemplateDropDownRequestModel>
{
    public GetListingTemplateDropDownRequestModelValidator()
    {
        RuleFor(p => p.MarketPlaceId).Required();
    }
}

public class
    GetListingTemplateDropDownRequestHandler : IRequestHandler<GetListingTemplateDropDownRequestModel,
        GetListingTemplateDropDownResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetListingTemplateDropDownRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetListingTemplateDropDownResponseModel> Handle(GetListingTemplateDropDownRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        Expression<Func<ListingTemplate, bool>> query = p =>
            p.MarketPlaceId == request.MarketPlaceId && p.MarketPlace.Team.UserId == userId;
        var list = await _context.ListingTemplates.GetAllReadOnly(query)
            .Select(ListingTemplateSelector.SelectorDropDown)
            .ToListAsync(cancellationToken);
        return new GetListingTemplateDropDownResponseModel()
        {
            Data = list
        };

    }

}

public class GetListingTemplateDropDownResponseModel
{
    public List<ListingTemplateDropDownDto> Data { get; set; }
}