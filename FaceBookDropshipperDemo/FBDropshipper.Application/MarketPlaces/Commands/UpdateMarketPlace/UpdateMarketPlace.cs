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

namespace FBDropshipper.Application.MarketPlaces.Commands.UpdateMarketPlace
{
    public class UpdateMarketPlaceRequestModel : IRequest<UpdateMarketPlaceResponseModel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MarketPlaceType MarketPlaceType { get; set; }
    }

    public class UpdateMarketPlaceRequestModelValidator : AbstractValidator<UpdateMarketPlaceRequestModel>
    {
        public UpdateMarketPlaceRequestModelValidator()
        {
            RuleFor(p => p.Id).Required();
            RuleFor(p => p.Name).Required();
            RuleFor(p => p.MarketPlaceType).IsInEnum();
        }
    }

    public class
        UpdateMarketPlaceRequestHandler : IRequestHandler<UpdateMarketPlaceRequestModel, UpdateMarketPlaceResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        public UpdateMarketPlaceRequestHandler(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public async Task<UpdateMarketPlaceResponseModel> Handle(UpdateMarketPlaceRequestModel request,
            CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();
            var marketplace = await 
                _context.MarketPlaces.GetByAsync(p => p.Id == request.Id && p.Team.UserId == userId, cancellationToken: cancellationToken);
            if (marketplace == null)
            {
                throw new NotFoundException(nameof(marketplace));
            }
            marketplace.Name = request.Name;
            marketplace.MarketPlaceType = request.MarketPlaceType.ToInt();
            _context.MarketPlaces.Update(marketplace);
            await _context.SaveChangesAsync(cancellationToken);
            return new UpdateMarketPlaceResponseModel(marketplace);
        }
    }

    public class UpdateMarketPlaceResponseModel : MarketPlaceDto
    {
        public UpdateMarketPlaceResponseModel(MarketPlace marketplace) : base(marketplace)
        {
            
        }
    }
}