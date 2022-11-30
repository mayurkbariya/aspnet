using System;
using System.Linq.Expressions;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.Cards.Models
{
    public class CardDto
    {
        public CardDto()
        {
            
        }

        public CardDto(UserCard merchant)
        {
            CardName = merchant.CardName;
            LastDigits = merchant.LastDigits;
            ExpiryDate = merchant.ExpiryDate;
            StripeToken = merchant.StripeToken;
            CardType = merchant.CardType;
        }
        public string CardName { get; set; }
        public string LastDigits { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string StripeToken { get; set; }
        public int CardType { get; set; }
    }

    public static class CardSelector
    {
        public static readonly Expression<Func<UserCard, CardDto>> Selector = p => new CardDto()
        {
            CardName = p.CardName,
            CardType = p.CardType,
            ExpiryDate = p.ExpiryDate,
            LastDigits = p.LastDigits,
            StripeToken = p.StripeToken
        };
    }
    
}