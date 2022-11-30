using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Cards.Models;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Cards.Queries.GetCard
{
    public class GetCardRequestModel : IRequest<GetCardResponseModel>
    {
    }

    public class GetCardRequestModelValidator : AbstractValidator<GetCardRequestModel>
    {
        public GetCardRequestModelValidator()
        {
        }
    }

    public class GetCardRequestHandler : IRequestHandler<GetCardRequestModel, GetCardResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        public GetCardRequestHandler(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public async Task<GetCardResponseModel> Handle(GetCardRequestModel request, CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();
            var merchant = await _context.UserCards.GetByWithSelectAsync(p => p.UserId == userId, CardSelector.Selector, cancellationToken: cancellationToken);
            return merchant == null ? new GetCardResponseModel() : merchant.CreateCopy<GetCardResponseModel>();
        }
    }

    public class GetCardResponseModel : CardDto
    {
        
    }
}