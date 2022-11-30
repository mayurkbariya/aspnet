using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Teams.Models;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Teams.Commands.UpdateTeam
{
    public class UpdateTeamRequestModel : IRequest<UpdateTeamResponseModel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateTeamRequestModelValidator : AbstractValidator<UpdateTeamRequestModel>
    {
        public UpdateTeamRequestModelValidator()
        {
            RuleFor(p => p.Id).Required();
            RuleFor(p => p.Name).Required().Max(255);
        }
    }

    public class
        UpdateTeamRequestHandler : IRequestHandler<UpdateTeamRequestModel, UpdateTeamResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        public UpdateTeamRequestHandler(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public async Task<UpdateTeamResponseModel> Handle(UpdateTeamRequestModel request,
            CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();
            var team = await _context.Teams.GetByReadOnlyAsync(
                p => p.Id == request.Id && p.UserId == userId,
                cancellationToken: cancellationToken);
            if (team == null)
            {
                throw new NotFoundException(nameof(team));
            }

            team.Name = request.Name;
            _context.Teams.Update(team);
            await _context.SaveChangesAsync(cancellationToken);
            return new UpdateTeamResponseModel(team);
        }

    }

    public class UpdateTeamResponseModel : TeamDto
    {
        public UpdateTeamResponseModel(Team team) : base(team)
        {
        }
    }
}