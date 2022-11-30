using System.Linq.Expressions;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.Orders.Models;

public class OrderDto
{
    public OrderDto()
    {
        
    }

    public OrderDto(Order order)
    {
        Id = order.Id;
        CreatedDate = order.CreatedDate;
        MarketPlaceId = order.MarketPlaceId;
        MarketPlace = order.MarketPlace?.Name;
        ProductListingId = order.ProductListingId;
        ProductListing = order.ProductListing.Title;
        OrderId = order.OrderId;
        OrderUrl = order.OrderUrl;
        Quantity = order.Quantity;
        SubTotal = order.SubTotal;
        Shipping = order.Shipping;
        Fee = order.Fee;
        SupplierOrderId = order.SupplierOrderId;
        SupplierCost = order.SupplierCost;
        TrackingCarrier = order.TrackingCarrier;
        TrackingNumber = order.TrackingNumber;
        OrderStatus = order.OrderStatus;
    }
    public DateTime CreatedDate { get; set; }
    public int Id { get; set; }
    public int MarketPlaceId { get; set; }
    public string MarketPlace { get; set; }
    public int ProductListingId { get; set; }
    public string ProductListing { get; set; }
    public string OrderId { get; set; }
    public string OrderUrl { get; set; }
    public int Quantity { get; set; }
    public double SubTotal { get; set; }
    public double Shipping { get; set; }
    public double Fee { get; set; }
    public string SupplierOrderId { get; set; }
    public double SupplierCost { get; set; }
    public int TrackingCarrier { get; set; }
    public string TrackingNumber { get; set; }
    public int OrderStatus { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public class OrderSelector
{
    public static Expression<Func<Order, OrderDto>> Selector = p => new OrderDto()
    {
        Fee = p.Fee,
        Id = p.Id,
        Quantity = p.Quantity,
        Shipping = p.Shipping,
        CreatedDate = p.CreatedDate,
        UpdatedDate = p.UpdatedDate,
        MarketPlace = p.MarketPlace.Name,
        OrderId = p.OrderId,
        OrderStatus = p.OrderStatus,
        OrderUrl = p.OrderUrl,
        ProductListing = p.ProductListing.Title,
        SubTotal = p.SubTotal,
        SupplierCost = p.SupplierCost,
        TrackingCarrier = p.TrackingCarrier,
        TrackingNumber = p.TrackingNumber,
        MarketPlaceId = p.MarketPlaceId,
        ProductListingId = p.ProductListingId,
        SupplierOrderId = p.SupplierOrderId,
    };
}