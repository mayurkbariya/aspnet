using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Cards.Models;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Constant;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Stripe;

namespace FBDropshipper.Application.Cards.Commands.UpdateCard
{
    public class UpdateCardRequestModel : IRequest<UpdateCardResponseModel>
    {
        public string Token { get; set; }
        public CardType CardType { get; set; }
        public string LastDigits { get; set; }
        public string Name { get; set; }
        public DateOnly ExpiryDate { get; set; }
    }

    public class UpdateCardRequestModelValidator : AbstractValidator<UpdateCardRequestModel>
    {
        public UpdateCardRequestModelValidator()
        {
            RuleFor(p => p.Name).Max(50);
            RuleFor(p => p.LastDigits).Pin();
            RuleFor(p => p.Token).Required();
            RuleFor(p => p.ExpiryDate).GreaterThan(DateOnly.MinValue);
            RuleFor(p => p.CardType).IsInEnum();
        }
    }

    public class UpdateCardRequestHandler : IRequestHandler<UpdateCardRequestModel, UpdateCardResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        public UpdateCardRequestHandler(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public async Task<UpdateCardResponseModel> Handle(UpdateCardRequestModel request, CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();
            var teamLeader = await _context.Users.GetByReadOnlyAsync(p => p.Id == userId, cancellationToken: cancellationToken);
            if (teamLeader == null)
            {
                throw new NotFoundException(nameof(RoleNames.TeamLeader));
            }
            var customerService = new CustomerService();
            var card = await _context.UserCards.GetByAsync(p => p.UserId == userId, cancellationToken: cancellationToken);
            if (card == null)
            {
                card = new UserCard()
                {
                    UserId = userId
                };
                var customerOptions = new CustomerCreateOptions {
                    Source = request.Token,
                    Email = teamLeader.Email,
                };
                Customer customer = await customerService.CreateAsync(customerOptions, cancellationToken: cancellationToken);
                var statusCode = customer.StripeResponse.StatusCode;
                if (statusCode == HttpStatusCode.Created || statusCode == HttpStatusCode.OK)
                {
                    card.StripeToken = customer.Id;
                    card.ExpiryDate = request.ExpiryDate.ToDateTime(TimeOnly.MinValue);
                    card.CardName = request.Name;
                    card.LastDigits = request.LastDigits;
                    card.CardType = request.CardType.ToInt();
                }
                _context.UserCards.Add(card);
            }
            else
            {
                var customerOptions = new CustomerUpdateOptions  {
                    Source = request.Token,
                    Email = teamLeader.Email,
                };
                Customer customer = await customerService.UpdateAsync(card.StripeToken, customerOptions, cancellationToken: cancellationToken);
                var statusCode = customer.StripeResponse.StatusCode;
                if (statusCode == HttpStatusCode.Created || statusCode == HttpStatusCode.OK)
                {
                    card.StripeToken = customer.Id;
                    card.ExpiryDate = request.ExpiryDate.ToDateTime(TimeOnly.MinValue);
                    card.CardName = request.Name;
                    card.LastDigits = request.LastDigits;
                    card.CardType = request.CardType.ToInt();
                }
                _context.UserCards.Update(card);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return new UpdateCardResponseModel(card);
        }
    }

    public class UpdateCardResponseModel : CardDto
    {
        public UpdateCardResponseModel(UserCard card) : base(card)
        {
        }
    }
}