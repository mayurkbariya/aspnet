using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.MarketPlaces.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.MarketPlaces.Commands.CreateMarketPlace
{
    public class CreateMarketPlaceRequestModel : IRequest<CreateMarketPlaceResponseModel>
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public MarketPlaceType MarketPlaceType { get; set; }
    }

    public class CreateMarketPlaceRequestModelValidator : AbstractValidator<CreateMarketPlaceRequestModel>
    {
        public CreateMarketPlaceRequestModelValidator()
        {
            RuleFor(p => p.TeamId).Required();
            RuleFor(p => p.Name).Required();
            RuleFor(p => p.MarketPlaceType).IsInEnum();
        }
    }

    public class
        CreateMarketPlaceRequestHandler : IRequestHandler<CreateMarketPlaceRequestModel, CreateMarketPlaceResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        public CreateMarketPlaceRequestHandler(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public async Task<CreateMarketPlaceResponseModel> Handle(CreateMarketPlaceRequestModel request,
            CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();
           
            var activeSub = await _context.UserSubscriptions.GetByAsync(p =>
                    p.UserId == userId && p.IsActive, 
                p => p.Include(pr => pr.Subscription),
                cancellationToken);
            if (activeSub == null)
            {
                throw new OkayButNotSuccessfulException("No Active Subscription Found");
            }
            var totalMarketPlace = await _context.MarketPlaces.ActiveCount(p => p.Team.UserId == userId, cancellationToken);
            if (totalMarketPlace >= activeSub.Subscription.TotalMarketPlace)
            {
                throw new OkayButNotSuccessfulException("No Free Slots left for Marketplaces. Delete existing and try again");
            }
            var team = await _context.Teams
                .GetByAsync(p => p.Id == request.TeamId && p.UserId == userId, 
                    cancellationToken: cancellationToken);
            if (team == null)
            {
                throw new NotFoundException(nameof(Team));
            }

            var marketplace = new MarketPlace()
            {
                Name = request.Name,
                TeamId = request.TeamId,
                MarketPlaceType = request.MarketPlaceType.ToInt()
            };
            _context.MarketPlaces.Add(marketplace);
            await _context.SaveChangesAsync(cancellationToken);
            return new CreateMarketPlaceResponseModel(marketplace);
        }

    }

    public class CreateMarketPlaceResponseModel : MarketPlaceDto
    {
        public CreateMarketPlaceResponseModel(MarketPlace place) : base(place)
        {
            
        }
    }
}