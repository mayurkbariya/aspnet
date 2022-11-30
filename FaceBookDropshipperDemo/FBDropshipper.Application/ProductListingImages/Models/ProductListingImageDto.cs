using System.Linq.Expressions;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.ProductListingImages.Models;

public class ProductListingImageDto
{
    public ProductListingImageDto()
    {
        
    }

    public ProductListingImageDto(ProductListingImage image)
    {
        Id = image.Id;
        Url = image.Url;
        CreatedDate = image.CreatedDate;
    }
    public int Id { get; set; }
    public string Url { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class ProductListingImageSelector
{
    public static Expression<Func<ProductListingImage, ProductListingImageDto>> Selector = p =>
        new ProductListingImageDto()
        {
            Id = p.Id,
            Url = p.Url,
            CreatedDate = p.CreatedDate
        };
}