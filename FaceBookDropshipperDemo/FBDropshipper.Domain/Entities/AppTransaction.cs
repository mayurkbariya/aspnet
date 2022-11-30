using System;

namespace FBDropshipper.Domain.Entities
{
    public class AppTransaction : Base
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string StripeSubscriptionId { get; set; }
        public string StripePaymentId { get; set; }
        public double Amount { get; set; }
        public double Fee { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}