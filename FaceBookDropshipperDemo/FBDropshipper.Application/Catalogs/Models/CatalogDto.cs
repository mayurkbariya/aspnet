using System.Linq.Expressions;
using FBDropshipper.Application.Shared;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.Catalogs.Models;

public class CatalogDropDownDto : DropDownDto<int>
{
    
}
public class CatalogDto
{
    public CatalogDto()
    {
        
    }

    public CatalogDto(Catalog catalog)
    {
        Id = catalog.Id;
        Name = catalog.Name;
        CatalogType = catalog.CatalogType;
        CreatedDate = catalog.CreatedDate;
        MarketPlaceId = catalog.MarketPlaceId;
        MarketPlace = catalog.MarketPlace?.Name;
    }

    public int? MarketPlaceId { get; set; }
    public string MarketPlace { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public int CatalogType { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class CatalogSelector
{
    public static readonly Expression<Func<Catalog, CatalogDto>> Selector = p => new CatalogDto()
    {
        Id = p.Id,
        Name = p.Name,
        CatalogType = p.CatalogType,
        CreatedDate = p.CreatedDate,
        MarketPlace = p.MarketPlace.Name,
        MarketPlaceId = p.MarketPlaceId
    };
    
    public static readonly Expression<Func<Catalog, CatalogDropDownDto>> SelectorDropDown = p => new CatalogDropDownDto()
    {
        Id = p.Id,
        Name = p.Name,
    };
}