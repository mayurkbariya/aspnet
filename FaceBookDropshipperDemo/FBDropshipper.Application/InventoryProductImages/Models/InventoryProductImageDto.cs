using System.Linq.Expressions;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.InventoryProductImages.Models;

public class InventoryProductImageDto
{
    public InventoryProductImageDto()
    {
        
    }

    public InventoryProductImageDto(InventoryProductImage image)
    {
        Id = image.Id;
        Url = image.Url;
        CreatedDate = image.CreatedDate;
    }
    public int Id { get; set; }
    public string Url { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class InventoryProductImageSelector
{
    public static Expression<Func<InventoryProductImage, InventoryProductImageDto>> Selector = p =>
        new InventoryProductImageDto()
        {
            Id = p.Id,
            Url = p.Url,
            CreatedDate = p.CreatedDate
        };
}