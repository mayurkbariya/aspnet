using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.ListingTemplates.Commands.DeleteListingTemplateBulk;

public class DeleteListingTemplateBulkRequestModel : IRequest<DeleteListingTemplateBulkResponseModel>
{
    public int[] Ids { get; set; }
}

public class DeleteListingTemplateBulkRequestModelValidator : AbstractValidator<DeleteListingTemplateBulkRequestModel>
{
    public DeleteListingTemplateBulkRequestModelValidator()
    {
        RuleFor(p => p.Ids).Required().Max(50);
    }
}

public class
    DeleteListingTemplateBulkRequestHandler : IRequestHandler<DeleteListingTemplateBulkRequestModel,
        DeleteListingTemplateBulkResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;

    public DeleteListingTemplateBulkRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<DeleteListingTemplateBulkResponseModel> Handle(DeleteListingTemplateBulkRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var templates = await 
            _context.ListingTemplates.GetAll(p =>
                request.Ids.Contains(p.Id) && p.MarketPlace.Team.UserId == userId)
                .ToListAsync(cancellationToken: cancellationToken);
        _context.ListingTemplates.RemoveRange(templates);
        await _context.SaveChangesAsync(cancellationToken);
        return new DeleteListingTemplateBulkResponseModel()
        {
            Count = templates.Count
        };
    }

}

public class DeleteListingTemplateBulkResponseModel
{
    public int Count { get; set; }
}