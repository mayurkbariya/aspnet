using System;
using System.Linq.Expressions;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.UserSubscriptions.Models
{
    
    public class UserSubscriptionDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string User { get; set; }
        public string StripeSubscriptionId { get; set; }
        public string StripePriceId { get; set; }
        public double Amount { get; set; }
        public string SubscriptionType { get; set; }
        public bool IsCancelled { get; set; }
        public bool CanExpire { get; set; }
        public string CreatedDate { get; set; }
        public string Status { get; set; }
        public string CurrentPeriodStart { get; set; }
        public string CurrentPeriodEnd { get; set; }
        public string Title { get; set; }
    }
    public class UserSubscriptionSelector
    {
        public static readonly Expression<Func<UserSubscription, UserSubscriptionDto>> MerchantSelector = p =>
            new UserSubscriptionDto()
            {
                Title = p.Subscription.Title,
                Amount = p.Amount,
                User = p.User.FullName,
                Status = p.Status,
                CanExpire = p.CanExpire,
                CreatedDate = p.CreatedDate.ToGeneralDateTime(),
                IsCancelled = p.IsCancelled,
                SubscriptionType = p.SubscriptionType,
                StripePriceId = p.StripePriceId,
                StripeSubscriptionId = p.StripeSubscriptionId,
                CurrentPeriodStart = p.CurrentPeriodStart.ToGeneralDateTime(),
                CurrentPeriodEnd = p.CurrentPeriodEnd.ToGeneralDateTime(),
                Id = p.Id,
                UserId = p.UserId
            };
        
    }
}