using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.TeamMembers.Commands.DeleteTeamMember;

public class DeleteTeamMemberRequestModel : IRequest<DeleteTeamMemberResponseModel>
{
    public string Id { get; set; }
}

public class DeleteTeamMemberRequestModelValidator : AbstractValidator<DeleteTeamMemberRequestModel>
{
    public DeleteTeamMemberRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    DeleteTeamMemberRequestHandler : IRequestHandler<DeleteTeamMemberRequestModel, DeleteTeamMemberResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public DeleteTeamMemberRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<DeleteTeamMemberResponseModel> Handle(DeleteTeamMemberRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetUserId();
        var teamMember = _context.TeamMembers.GetByReadOnly(p => p.Team.UserId == userId && p.UserId == request.Id,
            p => p.Include(pr => pr.User));
        if (teamMember == null)
        {
            throw new CannotDeleteException(nameof(teamMember));
        }

        _context.Users.Remove(teamMember.User);
        _context.TeamMembers.Remove(teamMember);
        await _context.SaveChangesAsync(cancellationToken);
        return new DeleteTeamMemberResponseModel();
    }

}

public class DeleteTeamMemberResponseModel
{

}