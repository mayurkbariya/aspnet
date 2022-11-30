using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.ListingTemplates.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.ListingTemplates.Commands.UpdateListingTemplate;

public class UpdateListingTemplateRequestModel : IRequest<UpdateListingTemplateResponseModel>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float ProfitPercent { get; set; }
    public int Quantity { get; set; }
    public float ShippingRate { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public string Header { get; set; }
}

public class UpdateListingTemplateRequestModelValidator : AbstractValidator<UpdateListingTemplateRequestModel>
{
    public UpdateListingTemplateRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
        RuleFor(p => p.Name).Required().Max(255);
        RuleFor(p => p.ProfitPercent).Min(0);
        RuleFor(p => p.Quantity).Min(0);
        RuleFor(p => p.DeliveryMethod).IsInEnum();
        RuleFor(p => p.ShippingRate).Min(0);
        RuleFor(p => p.Header).Required().Max(255);
    }
}

public class
    UpdateListingTemplateRequestHandler : IRequestHandler<UpdateListingTemplateRequestModel,
        UpdateListingTemplateResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public UpdateListingTemplateRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<UpdateListingTemplateResponseModel> Handle(UpdateListingTemplateRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var template =
            _context.ListingTemplates.GetByReadOnly(p =>
                !p.ProductLists.Any() &&
                p.Id == request.Id && p.MarketPlace.Team.UserId == userId);
        if (template == null)
        {
            throw new NotFoundException(nameof(template));
        }

        template.Header = request.Header;
        template.Name = request.Name;
        template.Quantity = request.Quantity;
        template.ProfitPercent = request.ProfitPercent;
        template.ShippingRate = request.ShippingRate;
        template.DeliveryMethod = request.DeliveryMethod.ToInt();
        _context.ListingTemplates.Update(template);
        await _context.SaveChangesAsync(cancellationToken);
        return new UpdateListingTemplateResponseModel(template);
    }
}

public class UpdateListingTemplateResponseModel : ListingTemplateDto
{
    public UpdateListingTemplateResponseModel(ListingTemplate template) : base(template)
    {
    }
}