using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Shared;
using FBDropshipper.Application.Users.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.TeamMembers.Queries.GetTeamMembersByTeamId
{
    public class GetTeamMembersByTeamIdRequestModel : GetPagedRequest<GetTeamMembersByTeamIdResponseModel>
    {
        public int TeamId { get; set; }
    }

    public class GetTeamMembersByTeamIdRequestModelValidator : PageRequestValidator<GetTeamMembersByTeamIdRequestModel>
    {
        public GetTeamMembersByTeamIdRequestModelValidator()
        {
            RuleFor(p => p.TeamId).Required();
        }
    }

    public class
        GetTeamMembersByTeamIdRequestHandler : IRequestHandler<GetTeamMembersByTeamIdRequestModel,
            GetTeamMembersByTeamIdResponseModel>
    {
        private readonly ApplicationDbContext _context;

        public GetTeamMembersByTeamIdRequestHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetTeamMembersByTeamIdResponseModel> Handle(GetTeamMembersByTeamIdRequestModel request,
            CancellationToken cancellationToken)
        {
            Expression<Func<User, bool>> query = p => p.TeamMember.TeamId == request.TeamId;
            if (request.Search.IsNotNullOrWhiteSpace())
            {
                query = query.AndAlso(p =>
                    p.Email.ToLower().Contains(request.Search) ||
                    p.FullName.ToLower().Contains(request.Search));
            }

            var list = await _context.Users.GetManyReadOnly(query, request)
                .Select(UserSelector.Selector)
                .ToListAsync(cancellationToken: cancellationToken);
            var count = await _context.Users.ActiveCount(query, cancellationToken);
            return new GetTeamMembersByTeamIdResponseModel()
            {
                Data = list,
                Count = count
            };
        }

    }

    public class GetTeamMembersByTeamIdResponseModel
    {
        public List<UserDto> Data { get; set; }
        public int Count { get; set; }
    }
}