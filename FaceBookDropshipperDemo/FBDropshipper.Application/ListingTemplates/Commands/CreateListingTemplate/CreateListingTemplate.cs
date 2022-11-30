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

namespace FBDropshipper.Application.ListingTemplates.Commands.CreateListingTemplate;

public class CreateListingTemplateRequestModel : IRequest<CreateListingTemplateResponseModel>
{
    public int MarketPlaceId { get; set; }
    public string Name { get; set; }
    public float ProfitPercent { get; set; }
    public int Quantity { get; set; }
    public float ShippingRate { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public string Header { get; set; }
}

public class CreateListingTemplateRequestModelValidator : AbstractValidator<CreateListingTemplateRequestModel>
{
    public CreateListingTemplateRequestModelValidator()
    {
        RuleFor(p => p.MarketPlaceId).Required();
        RuleFor(p => p.Name).Required().Max(255);
        RuleFor(p => p.ProfitPercent).Min(0);
        RuleFor(p => p.Quantity).Min(0);
        RuleFor(p => p.DeliveryMethod).IsInEnum();
        RuleFor(p => p.ShippingRate).Min(0);
        RuleFor(p => p.Header).Required().Max(255);
    }
}

public class
    CreateListingTemplateRequestHandler : IRequestHandler<CreateListingTemplateRequestModel,
        CreateListingTemplateResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public CreateListingTemplateRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<CreateListingTemplateResponseModel> Handle(CreateListingTemplateRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var marketplace =await 
            _context.MarketPlaces.GetByReadOnlyAsync(p => p.Id == request.MarketPlaceId && p.Team.UserId == userId, cancellationToken: cancellationToken);
        if (marketplace == null)
        {
            throw new NotFoundException(nameof(marketplace));
        }

        var template = new ListingTemplate()
        {
            Header = request.Header,
            Name = request.Name,
            Quantity = request.Quantity,
            DeliveryMethod = request.DeliveryMethod.ToInt(),
            ProfitPercent = request.ProfitPercent,
            MarketPlaceId = request.MarketPlaceId,
            ShippingRate = request.ShippingRate,
        };
        _context.ListingTemplates.Add(template);
        await _context.SaveChangesAsync(cancellationToken);
        template.MarketPlace = marketplace;
        return new CreateListingTemplateResponseModel(template);
    }

}

public class CreateListingTemplateResponseModel : ListingTemplateDto
{
    public CreateListingTemplateResponseModel(ListingTemplate template) : base(template)
    {
        
    }
}