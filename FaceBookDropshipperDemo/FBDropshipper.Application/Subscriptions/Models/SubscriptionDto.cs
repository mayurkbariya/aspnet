using System;
using System.Linq.Expressions;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.Subscriptions.Models
{
    public class SubscriptionDto
    {
        public SubscriptionDto()
        {
            
        }

        public SubscriptionDto(Subscription subscription)
        {
            Id = subscription.Id;
            Title = subscription.Title;
            Description = subscription.Description;
            Amount = subscription.Amount;
            SubscriptionType = subscription.SubscriptionType;
            TotalProducts = subscription.TotalProducts;
            TotalMarketPlace = subscription.TotalMarketPlace;
            TotalTeamMembers = subscription.TotalTeamMembers;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int SubscriptionType { get; set; }
        public int TotalProducts { get; set; }
        public int TotalMarketPlace { get; set; }
        public int TotalTeamMembers { get; set; }
    }

    public static class SubscriptionSelector
    {
        public static Expression<Func<Subscription, SubscriptionDto>> Selector = p => new SubscriptionDto()
        {
            Amount = p.Amount,
            Description = p.Description,
            Id = p.Id,
            Title = p.Title,
            SubscriptionType = p.SubscriptionType,
            TotalProducts = p.TotalProducts,
            TotalMarketPlace = p.TotalMarketPlace,
            TotalTeamMembers = p.TotalTeamMembers
        };
    }
}