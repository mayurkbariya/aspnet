using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBDropshipper.Application.Interfaces;
using Stripe;

namespace FBDropshipper.Infrastructure.Service
{
    public class StripeService : IStripeService
    {
        public async Task<List<Price>> GetAllPrices()
        {
            var service = new PriceService();
            var list = await service.ListAsync(new PriceListOptions()
            {
                Expand = new List<string>()
                {
                    "data.product"
                },
                Active = true
            });
            return list.Data;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var service = new ProductService();
            var list = await service.ListAsync();
            return list.Data;
        }

        public async Task<List<Subscription>> GetAllSubscriptions()
        {
            var service = new SubscriptionService();
            var list = await service.ListAsync();
            return list.Data;
        }

        public async Task<bool> CancelSubscription(string subId)
        {
            var service = new SubscriptionService();
            var sub = await service.CancelAsync(subId,new SubscriptionCancelOptions()
            {
                InvoiceNow = false,
                Prorate = false,
            });
            return true;
        }

        public async Task<Subscription> SubscribeToSubscription(string customerId, string priceId)
        {
            var options = new SubscriptionCreateOptions
            {
                Customer = customerId,
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Price = priceId,
                    },
                },
            };
            var service = new SubscriptionService();
            return await service.CreateAsync(options);
        }

        public async Task<Subscription> UpdateSubscription(string subscriptionId, string priceId)
        {
            var service = new SubscriptionService();
            Subscription subscription = await service.GetAsync(subscriptionId);

            var items = new List<SubscriptionItemOptions>
            {
                new SubscriptionItemOptions
                {
                    Id = subscription.Items.Data[0].Id,
                    Price = priceId,
                },
            };
            var options = new SubscriptionUpdateOptions
            {
                Items = items,
                ProrationBehavior = "none",
            };
            subscription = await service.UpdateAsync(subscriptionId, options);
            return subscription;
        }


        public async Task<string> CreateProduct(string name,string description)
        {
            var options = new ProductCreateOptions
            {
                Name = name,
                Description = description
            };
            var service = new ProductService();
            var product = await service.CreateAsync(options);
            return product.Id;
        }
        public async Task<string> CreatePrice(string productId, long price, string interval)
        {
            var options = new PriceCreateOptions
            {
                Product = productId,
                UnitAmount = price,
                Currency = "usd",
                Recurring = new PriceRecurringOptions
                {
                    Interval = interval,
                },
            };
            var service = new PriceService();
            var priceEntity = await service.CreateAsync(options);
            return priceEntity.Id;
        }

        public async Task<string> UpdateProduct(string id, string name, string description)
        {
            var options = new ProductUpdateOptions
            {
                Name = name,
                Description = description
            };
            var service = new ProductService();
            await service.UpdateAsync(id, options);
            return id;
        }

        private async Task MakePriceInActive(string id)
        {
            var service = new PriceService();
            var options = new PriceUpdateOptions
            {
                Active = false
            };
            await service.UpdateAsync(
                id,
                options
            );
        }
        public async Task<string> UpdatePrice(string id, string productId, long price, string interval, bool makePriceInActive = false)
        {
            if (makePriceInActive)
            {
                await MakePriceInActive(id);
                return await CreatePrice(productId, price, interval);
            }
            return id;
        }

        public async Task<Tuple<Subscription, string>> SubscribeSubscription(string cardToken, long amount)
        {
            var options = new SubscriptionCreateOptions
            {
                Customer = "cus_IoN4SGIpLzP04w",
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Price = "price_1ICW0l2eZvKYlo2CUG1qEbsa",
                    },
                },
            };
            var service = new SubscriptionService();
            var sub = await service.CreateAsync(options);
            return Tuple.Create<Subscription, string>(sub, null);
        }
        public async Task<Tuple<PaymentIntent, string>> ProcessPayment(string cardToken, long amount, long fee,
            string merchantToken)
        {
            try
            {
                var options = new TokenCreateOptions
                {
                    Customer = cardToken,
                };

                var tokenRequestOptions = new RequestOptions
                {
                    StripeAccount = merchantToken,
                };

                var tokenService = new TokenService();
                var token = await tokenService.CreateAsync(options, tokenRequestOptions);

                var customerCreateOptions = new CustomerCreateOptions
                {
                    Source = token.Id,
                };
                var customerRequestOptions = new RequestOptions
                {
                    StripeAccount = merchantToken,
                };

                var customerService = new CustomerService();
                var customer = await customerService.CreateAsync(customerCreateOptions, customerRequestOptions);

                var service = new PaymentIntentService();


                // var service = new ChargeService();
                var createOptions = new PaymentIntentCreateOptions()
                {
                    Customer = customer.Id,
                    Amount = amount,
                    Currency = "usd",
                    ApplicationFeeAmount = fee,
                    PaymentMethod = customer.DefaultSourceId,
                    Confirm = true
                };
                var requestOptions = new RequestOptions {StripeAccount = merchantToken};
                PaymentIntent result = await service.CreateAsync(createOptions, requestOptions);
                return Tuple.Create<PaymentIntent, string>(result, null);
            }
            catch (Exception e)
            {
                return new Tuple<PaymentIntent, string>(null, e.Message);
            }
        }

        public async Task<Tuple<Account, string>> CreateAccount(string email, string ipAddress)
        {
            try
            {
                var options = new AccountCreateOptions
                {
                    Type = "custom",
                    Country = "US",
                    Email = email,
                    Capabilities = new AccountCapabilitiesOptions()
                    {
                        CardPayments = new AccountCapabilitiesCardPaymentsOptions()
                        {
                            Requested = true
                        },
                        Transfers = new AccountCapabilitiesTransfersOptions()
                        {
                            Requested = true
                        }
                    },
                    TosAcceptance = new AccountTosAcceptanceOptions()
                    {
                        Date = DateTime.UtcNow,
                        Ip = ipAddress,
                    },
                };
                var service = new AccountService();
                return Tuple.Create(await service.CreateAsync(options), "");
            }
            catch (Exception e)
            {
                return new Tuple<Account, string>(null, e.Message);
            }
        }

        public string GenerateUrl(string token, string host)
        {
            //Todo Change to local host

            var redirectUrl = "&redirect_uri=http%3A%2F%2Flocalhost%3A4200%2F%23%2Fmerchant-registration";
            if (!host.Contains("localhost"))
            {
                redirectUrl = "";
            }

            var url =
                $"https://connect.stripe.com/express/oauth/authorize?response_type=code&client_id={StripeConfiguration.ClientId}&state={token}&scope=read_write" +
                redirectUrl;
            return url;
        }

        public async Task<Tuple<Account, string>> GetAccountByAccessToken(string accessToken)
        {
            try
            {
                var options = new OAuthTokenCreateOptions
                {
                    GrantType = "authorization_code",
                    Code = accessToken,
                };
                var service = new AccountService();
                var authService = new OAuthTokenService();
                var authResponse = await authService.CreateAsync(options);
                var userId = authResponse.StripeUserId;
                var getOpt = new AccountGetOptions();
                getOpt.AddExpand("business_profile.support_address");
                getOpt.AddExpand("individual");
                getOpt.AddExpand("company");
                var account = await service.GetAsync(userId, getOpt);
                return new Tuple<Account, string>(account, null);
            }
            catch (Exception e)
            {
                return new Tuple<Account, string>(null, e.Message);
            }
        }

        public async Task<Tuple<Refund, string>> RefundSale(string chargeId, string accountId)
        {
            try
            {
                var paymentId = chargeId;
                if (chargeId.StartsWith("pi_"))
                {
                    chargeId = null;
                }
                else
                {
                    paymentId = null;
                }

                var refunds = new RefundService();
                var refundOptions = new RefundCreateOptions
                {
                    Charge = chargeId,
                    PaymentIntent = paymentId
                };
                var requestOptions = new RequestOptions();
                requestOptions.StripeAccount = accountId;
                Refund refund = await refunds.CreateAsync(refundOptions, requestOptions);
                return new Tuple<Refund, string>(refund, "");
            }
            catch (Exception e)
            {
                return new Tuple<Refund, string>(null, e.Message);
            }
        }

        public async Task<Tuple<Account, string>> GetAccountById(string requestId)
        {
            try
            {
                var service = new AccountService();
                var userId = requestId;
                var getOpt = new AccountGetOptions();
                getOpt.AddExpand("business_profile.support_address");
                getOpt.AddExpand("individual");
                getOpt.AddExpand("company");
                var account = await service.GetAsync(userId, getOpt);
                return new Tuple<Account, string>(account, null);
            }
            catch (Exception e)
            {
                return new Tuple<Account, string>(null, e.Message);
            }
        }
    }
}