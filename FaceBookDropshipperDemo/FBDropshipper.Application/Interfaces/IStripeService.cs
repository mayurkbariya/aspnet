using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;

namespace FBDropshipper.Application.Interfaces
{
    public interface IStripeService
    {
        Task<List<Price>> GetAllPrices();
        Task<List<Product>> GetAllProducts();
        Task<List<Subscription>> GetAllSubscriptions();
        Task<bool> CancelSubscription(string subId);
        Task<Subscription> SubscribeToSubscription(string customerId, string priceId);
        Task<Subscription> UpdateSubscription(string subscriptionId, string priceId);
        Task<string> CreateProduct(string name, string description);
        Task<string> CreatePrice(string productId, long price, string interval);
        Task<string> UpdateProduct(string id, string name, string description);
        Task<string> UpdatePrice(string id, string productId, long price, string interval, bool makePriceInActive = false);
        public Task<Tuple<PaymentIntent, string>> ProcessPayment(string cardToken,long amount, long fee, string merchantToken);
        public Task<Tuple<Account, string>> CreateAccount(string email, string ipAddress);
        public string GenerateUrl(string token, string host);
        public Task<Tuple<Account, string>> GetAccountByAccessToken(string accessToken);
        public Task<Tuple<Refund, string>> RefundSale(string paymentIntentId, string accountId);
        public Task<Tuple<Account, string>> GetAccountById(string requestId);
    }
}