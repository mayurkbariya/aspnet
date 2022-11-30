using System.Linq.Expressions;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.CatalogProductImages.Models;

public class CatalogProductImageDto
{
    public CatalogProductImageDto()
    {
        
    }

    public CatalogProductImageDto(CatalogProductImage image)
    {
        Id = image.Id;
        Url = image.Url;
        CreatedDate = image.CreatedDate;
        Order = image.Order;
    }
    public int Id { get; set; }
    public string Url { get; set; }
    public DateTime CreatedDate { get; set; }
    public int Order { get; set; }
}

public class CatalogProductImageSelector
{
    public static readonly Expression<Func<CatalogProductImage, CatalogProductImageDto>> Selector = p =>
        new CatalogProductImageDto()
        {
            Id = p.Id,
            Url = p.Url,
            CreatedDate = p.CreatedDate,
            Order = p.Order,
        };
}