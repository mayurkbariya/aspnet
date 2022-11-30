using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Shared;
using FBDropshipper.Application.Users.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Constant;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.Users.Queries.GetUsers
{
    public class GetUsersRequestModel : GetPagedRequest<GetUsersResponseModel>
    {

    }

    public class GetUsersRequestModelValidator : PageRequestValidator<GetUsersRequestModel>
    {
        public GetUsersRequestModelValidator()
        {

        }
    }

    public class
        GetUsersRequestHandler : IRequestHandler<GetUsersRequestModel, GetUsersResponseModel>
    {
        private readonly ApplicationDbContext _context;

        public GetUsersRequestHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetUsersResponseModel> Handle(GetUsersRequestModel request,
            CancellationToken cancellationToken)
        {
            Expression<Func<User, bool>> query = p => p.UserRoles.Any(pr => pr.Role.Name == RoleNames.TeamLeader);
            if (request.Search.IsNotNullOrWhiteSpace())
            {
                query = query.AndAlso(p =>
                    p.Email.ToLower().Contains(request.Search) || p.FullName.ToLower().Contains(request.Search));
            }

            var list = await _context.Users.GetManyReadOnly(query, request)
                .Select(UserSelector.Selector)
                .ToListAsync(cancellationToken);
            var count = await _context.Users.ActiveCount(query, cancellationToken);
            return new GetUsersResponseModel()
            {
                Data = list,
                Count = count
            };
        }

    }

    public class GetUsersResponseModel
    {
        public List<UserDto> Data { get; set; }
        public int Count { get; set; }
    }
}