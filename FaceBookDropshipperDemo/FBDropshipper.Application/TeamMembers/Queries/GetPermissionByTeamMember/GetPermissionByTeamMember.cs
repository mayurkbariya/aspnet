using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.TeamMembers.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.TeamMembers.Queries.GetPermissionByTeamMember;

public class GetPermissionByTeamMemberRequestModel : IRequest<GetPermissionByTeamMemberResponseModel>
{
    public string Id { get; set; }
}

public class GetPermissionByTeamMemberRequestModelValidator : AbstractValidator<GetPermissionByTeamMemberRequestModel>
{
    public GetPermissionByTeamMemberRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    GetPermissionByTeamMemberRequestHandler : IRequestHandler<GetPermissionByTeamMemberRequestModel,
        GetPermissionByTeamMemberResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetPermissionByTeamMemberRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetPermissionByTeamMemberResponseModel> Handle(GetPermissionByTeamMemberRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetUserId();
        var user = await _context.Users
            .GetByWithSelectAsync(p => p.TeamMember.Team.UserId == userId, 
                PermissionSelector.Selector,
                cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new NotFoundException(nameof(user));
        }
        return user.CreateCopy<GetPermissionByTeamMemberResponseModel>();
    }

}

public class GetPermissionByTeamMemberResponseModel : TeamMemberPermissionDto
{

}