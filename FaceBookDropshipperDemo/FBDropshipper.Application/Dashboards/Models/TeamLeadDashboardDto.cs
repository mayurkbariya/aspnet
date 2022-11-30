using System.Linq.Expressions;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.Dashboards.Models;

public class TeamLeadDashboardDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Inventory { get; set; }
    public int Listings { get; set; }
    public int Orders { get; set; }
}

public class DashboardSelector
{
    public static readonly Expression<Func<MarketPlace, TeamLeadDashboardDto>> TeamLeadSelector = p => new TeamLeadDashboardDto()
    {
        Id = p.Id,
        Inventory = p.InventoryProducts.Count(),
        Listings = p.ProductLists.Count(),
        Orders = p.Orders.Count(),
        Name = p.Name
    };
}