using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.MarketPlaces.Models;
using FBDropshipper.Application.Shared;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.MarketPlaces.Queries.GetMarketPlaces
{
    public class GetMarketPlacesRequestModel : GetPagedRequest<GetMarketPlacesResponseModel>
    {
        public int TeamId { get; set; }
    }

    public class GetMarketPlacesRequestModelValidator : PageRequestValidator<GetMarketPlacesRequestModel>
    {
        public GetMarketPlacesRequestModelValidator()
        {
            RuleFor(p => p.TeamId).Required();
        }
    }

    public class
        GetMarketPlacesRequestHandler : IRequestHandler<GetMarketPlacesRequestModel, GetMarketPlacesResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        public GetMarketPlacesRequestHandler(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public async Task<GetMarketPlacesResponseModel> Handle(GetMarketPlacesRequestModel request,
            CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();
            Expression<Func<MarketPlace, bool>> query = p => 
                p.Team.UserId == userId &&
                p.TeamId == request.TeamId;
            if (request.Search.IsNotNullOrWhiteSpace())
            {
                query = query.AndAlso(p => p.Name.ToLower().Contains(request.Search));
            }

            var list = await _context.MarketPlaces.GetManyReadOnly(query, request)
                .Select(MarketPlaceSelector.Selector)
                .ToListAsync(cancellationToken: cancellationToken);
            var count = await _context.MarketPlaces.ActiveCount(query, cancellationToken);
            return new GetMarketPlacesResponseModel()
            {
                Data = list,
                Count = count
            };
        }

    }

    public class GetMarketPlacesResponseModel
    {
        public List<MarketPlaceDto> Data { get; set; }
        public int Count { get; set; }
    }
}