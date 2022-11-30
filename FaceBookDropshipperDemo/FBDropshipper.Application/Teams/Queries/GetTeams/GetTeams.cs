using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Shared;
using FBDropshipper.Application.Teams.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.Teams.Queries.GetTeams
{
    public class GetTeamsRequestModel : GetPagedRequest<GetTeamsResponseModel>
    {

    }

    public class GetTeamsRequestModelValidator : PageRequestValidator<GetTeamsRequestModel>
    {
        public GetTeamsRequestModelValidator()
        {

        }
    }

    public class
        GetTeamsRequestHandler : IRequestHandler<GetTeamsRequestModel, GetTeamsResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        public GetTeamsRequestHandler(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public async Task<GetTeamsResponseModel> Handle(GetTeamsRequestModel request,
            CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();
            Expression<Func<Team, bool>> query = p => p.UserId == userId;
            if (request.Search.IsNotNullOrWhiteSpace())
            {
                query = query.AndAlso(p => p.Name.ToLower().Contains(request.Search));
            }

            var list = await _context.Teams.GetManyReadOnly(query, request)
                .Select(TeamSelector.Selector)
                .ToListAsync(cancellationToken: cancellationToken);
            var count = await _context.Teams.ActiveCount(query, cancellationToken);
            return new GetTeamsResponseModel()
            {
                Data = list,
                Count = count
            };
        }

    }

    public class GetTeamsResponseModel
    {
        public List<TeamDto> Data { get; set; }
        public int Count { get; set; }
    }
}