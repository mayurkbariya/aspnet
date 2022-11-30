using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.MarketPlaces.Commands.DeleteMarketPlace
{
    public class DeleteMarketPlaceRequestModel : IRequest<DeleteMarketPlaceResponseModel>
    {
        public int Id { get; set; }
    }

    public class DeleteMarketPlaceRequestModelValidator : AbstractValidator<DeleteMarketPlaceRequestModel>
    {
        public DeleteMarketPlaceRequestModelValidator()
        {
            RuleFor(p => p.Id).Required();
        }
    }

    public class
        DeleteMarketPlaceRequestHandler : IRequestHandler<DeleteMarketPlaceRequestModel, DeleteMarketPlaceResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        public DeleteMarketPlaceRequestHandler(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public async Task<DeleteMarketPlaceResponseModel> Handle(DeleteMarketPlaceRequestModel request,
            CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();
            var marketplace = await 
                _context.MarketPlaces.GetByAsync(p => p.Id == request.Id && p.Team.UserId == userId, cancellationToken: cancellationToken);
            if (marketplace == null)
            {
                throw new NotFoundException(nameof(marketplace));
            }
            _context.MarketPlaces.Remove(marketplace);
            await _context.SaveChangesAsync(cancellationToken);
            return new DeleteMarketPlaceResponseModel();
        }

    }

    public class DeleteMarketPlaceResponseModel
    {

    }
}