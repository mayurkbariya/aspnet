using System;
using FBDropshipper.Domain.Interfaces;

namespace FBDropshipper.Domain.Entities
{
    public class UserSubscription : Base
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        public string StripeSubscriptionId { get; set; }
        public string StripePriceId { get; set; }
        public double Amount { get; set; }
        public string SubscriptionType { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsActive { get; set; }
        public bool CanCancel { get; set; }
        public bool CanExpire { get; set; }
        public DateTime? CancelledDate { get; set; }
        /// <summary>
        /// This Property is used to Verify if the Subscription is active or not
        /// </summary>
        public string Status { get; set; }
        public DateTime? CurrentPeriodStart { get; set; }
        public DateTime? CurrentPeriodEnd { get; set; }
    }
}