using System.Collections.Generic;

namespace FBDropshipper.Domain.Entities
{
    public class Subscription : Base
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        /// <summary>
        /// <see cref="SubscriptionType"/>
        /// </summary>
        public int SubscriptionType { get; set; }
        public string StripeProductId { get; set; }
        public string StripePriceId { get; set; }
        public int TotalProducts { get; set; }
        public int TotalMarketPlace { get; set; }
        public int TotalTeamMembers { get; set; }
        public IEnumerable<UserSubscription> UserSubscriptions { get; set; }
    }
}