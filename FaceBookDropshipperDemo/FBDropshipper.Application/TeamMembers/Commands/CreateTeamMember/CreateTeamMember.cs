using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Templates;
using FBDropshipper.Application.Users.Models;
using FBDropshipper.Domain.Constant;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.TeamMembers.Commands.CreateTeamMember
{
    public class CreateTeamMemberRequestModel : IRequest<CreateTeamMemberResponseModel>
    {
        public int TeamId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class CreateTeamMemberRequestModelValidator : AbstractValidator<CreateTeamMemberRequestModel>
    {
        public CreateTeamMemberRequestModelValidator()
        {
            RuleFor(p => p.TeamId).Required();
            RuleFor(p => p.FullName).Required().Max(255);
            RuleFor(p => p.Email).Required()
                .EmailAddress().Max(255);
            RuleFor(p => p.Password).Password();
            RuleFor(p => p.ConfirmPassword).Matches(p => p.Password);

        }
    }

    public class
        CreateTeamMemberRequestHandler : IRequestHandler<CreateTeamMemberRequestModel, CreateTeamMemberResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IUrlService _urlService;
        public CreateTeamMemberRequestHandler(ApplicationDbContext context, ISessionService sessionService, UserManager<User> userManager, IEmailService emailService, IUrlService urlService)
        {
            _context = context;
            _sessionService = sessionService;
            _userManager = userManager;
            _emailService = emailService;
            _urlService = urlService;
        }

        public async Task<CreateTeamMemberResponseModel> Handle(CreateTeamMemberRequestModel request,
            CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();

            var sub = await _context.UserSubscriptions.GetByAsync(p =>
                p.UserId == userId && p.IsActive, 
                p => p.Include(pr => pr.Subscription),
                cancellationToken);
            if (sub == null)
            {
                throw new OkayButNotSuccessfulException("No Active Subscription Found");
            }

            var totalMembers = await _context.TeamMembers.ActiveCount(p => p.Team.UserId == userId, cancellationToken);
            if (totalMembers >= sub.Subscription.TotalTeamMembers)
            {
                throw new OkayButNotSuccessfulException("No Free Slots left for Team Members. Delete existing and try again");
            }
            var emailUpper = request.Email.ToUpper();
            
            var isExists = await _context.Users.ActiveAny(p => p.NormalizedEmail == emailUpper);
            if (isExists)
            {
                throw new AlreadyExistsException(nameof(User));
            }
            var team = await _context.Teams.GetByAsync(p => p.Id == request.TeamId && p.UserId == userId, cancellationToken: cancellationToken);
            if (team == null)
            {
                throw new NotFoundException(nameof(team));
            }
            var teamMemberId = await _context.Roles.GetByWithSelectAsync(p => p.Name == RoleNames.TeamMember, p => p.Id,
                cancellationToken: cancellationToken);
            if (string.IsNullOrWhiteSpace(teamMemberId))
            {
                throw new NotFoundException(nameof(RoleNames));
            }
            var user = new User()
            {
                Email = request.Email,
                FullName = request.FullName,
                NormalizedEmail = emailUpper,
                UserName = request.Email,
                NormalizedUserName = emailUpper,
                UserRoles = new List<UserRole>()
                {
                    new()
                    {
                        RoleId = teamMemberId
                    }
                },
                TeamMember = new TeamMember()
                {
                    CanLogin = true,
                    TeamId = request.TeamId,
                },
                IsEnabled = false
            };
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, request.Password);
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendEmailByTemplate(user.Email, EmailTemplateType.TeamInviteTemplate, new
            {
                username = user.FullName,
                verificationLink = _urlService.GetVerificationUrl(user.Id,token)
            });
            return new CreateTeamMemberResponseModel(user);
        }
    }

    public class CreateTeamMemberResponseModel : UserDto
    {
        public CreateTeamMemberResponseModel(User user) : base(user)
        {
            
        }
    }
}