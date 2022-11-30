using System.Linq.Expressions;
using FBDropshipper.Application.Shared;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.ListingTemplates.Models;

public class ListingTemplateDropDownDto : DropDownDto<int>
{
}

public class ListingTemplateDto
{
    public ListingTemplateDto()
    {
    }

    public ListingTemplateDto(ListingTemplate template)
    {
        Id = template.Id;
        MarketPlace = template.MarketPlace?.Name;
        MarketPlaceId = template.MarketPlaceId;
        Name = template.Name;
        ProfitPercent = template.ProfitPercent;
        Quantity = template.Quantity;
        ShippingRate = template.ShippingRate;
        CreatedDate = template.CreatedDate;
        DeliveryMethod = template.DeliveryMethod;
        Header = template.Header;
    }

    public DateTime CreatedDate { get; set; }
    public int Id { get; set; }
    public int MarketPlaceId { get; set; }
    public string MarketPlace { get; set; }
    public string Name { get; set; }
    public float ProfitPercent { get; set; }
    public int Quantity { get; set; }
    public float ShippingRate { get; set; }
    public int DeliveryMethod { get; set; }
    public string Header { get; set; }
}

public class ListingTemplateSelector
{
    public static readonly Expression<Func<ListingTemplate, ListingTemplateDropDownDto>> SelectorDropDown = p =>
        new ListingTemplateDropDownDto()
        {
            Id = p.Id,
            Name = p.Name
        };

    public static readonly Expression<Func<ListingTemplate, ListingTemplateDto>> Selector = p => new ListingTemplateDto()
    {
        Header = p.Header,
        Id = p.Id,
        Name = p.Name,
        Quantity = p.Quantity,
        CreatedDate = p.CreatedDate,
        DeliveryMethod = p.DeliveryMethod,
        MarketPlace = p.MarketPlace.Name,
        ProfitPercent = p.ProfitPercent,
        ShippingRate = p.ShippingRate,
        MarketPlaceId = p.MarketPlaceId
    };
}