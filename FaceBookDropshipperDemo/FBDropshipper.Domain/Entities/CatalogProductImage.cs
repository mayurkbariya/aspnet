using FBDropshipper.Domain.Enum;

namespace FBDropshipper.Domain.Entities;

public class CatalogProductImage : Base
{
    public int CatalogProductId { get; set; }
    public CatalogProduct CatalogProduct { get; set; }
    public string Url { get; set; }
    public int Order { get; set; }
}