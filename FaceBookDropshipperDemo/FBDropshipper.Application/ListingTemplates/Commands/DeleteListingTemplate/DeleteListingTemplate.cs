using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.ListingTemplates.Commands.DeleteListingTemplate;

public class DeleteListingTemplateRequestModel : IRequest<DeleteListingTemplateResponseModel>
{
    public int Id { get; set; }
}

public class DeleteListingTemplateRequestModelValidator : AbstractValidator<DeleteListingTemplateRequestModel>
{
    public DeleteListingTemplateRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    DeleteListingTemplateRequestHandler : IRequestHandler<DeleteListingTemplateRequestModel,
        DeleteListingTemplateResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public DeleteListingTemplateRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<DeleteListingTemplateResponseModel> Handle(DeleteListingTemplateRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var template =
            _context.ListingTemplates.GetByReadOnly(p =>
                p.Id == request.Id && p.MarketPlace.Team.UserId == userId);
        if (template == null)
        {
            throw new CannotDeleteException(nameof(template));
        }
        _context.ListingTemplates.Remove(template);
        await _context.SaveChangesAsync(cancellationToken);
        return new DeleteListingTemplateResponseModel();
    }

}

public class DeleteListingTemplateResponseModel
{

}