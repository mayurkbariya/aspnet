using System;
using FBDropshipper.Domain.Interfaces;

namespace FBDropshipper.Domain.Entities
{
    public class UserCard : IBase
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string CardName { get; set; }
        public string LastDigits { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string StripeToken { get; set; }
        public int CardType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}