using System.Linq.Expressions;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.ListingTemplates.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.ListingTemplates.Queries.GetListingTemplateById;

public class GetListingTemplateByIdRequestModel : IRequest<GetListingTemplateByIdResponseModel>
{
    public int Id { get; set; }
}

public class GetListingTemplateByIdRequestModelValidator : AbstractValidator<GetListingTemplateByIdRequestModel>
{
    public GetListingTemplateByIdRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    GetListingTemplateByIdRequestHandler : IRequestHandler<GetListingTemplateByIdRequestModel,
        GetListingTemplateByIdResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetListingTemplateByIdRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetListingTemplateByIdResponseModel> Handle(GetListingTemplateByIdRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        Expression<Func<ListingTemplate, bool>> query = p =>
            p.MarketPlaceId == request.Id && p.MarketPlace.Team.UserId == userId;
        var template = await _context.ListingTemplates.GetByWithSelectAsync(query, ListingTemplateSelector.Selector, cancellationToken: cancellationToken);
        if (template == null)
        {
            throw new NotFoundException(nameof(template));
        }

        return template.CreateCopy<GetListingTemplateByIdResponseModel>();
    }

}

public class GetListingTemplateByIdResponseModel : ListingTemplateDto
{

}