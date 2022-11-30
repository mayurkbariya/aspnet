using System.Collections.Generic;
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
using Microsoft.Extensions.Options;

namespace FBDropshipper.Application.Users.Commands.RegisterUser
{
    public class RegisterUserRequestModel : IRequest<RegisterUserResponseModel>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class RegisterUserRequestModelValidator : AbstractValidator<RegisterUserRequestModel>
    {
        public RegisterUserRequestModelValidator()
        {
            RuleFor(p => p.FullName).Required().Max(255);
            RuleFor(p => p.Email).Required()
                .EmailAddress().Max(255);
            RuleFor(p => p.Password).Password();
            RuleFor(p => p.ConfirmPassword).Matches(p => p.Password);
        }
    }

    public class
        RegisterUserRequestHandler : IRequestHandler<RegisterUserRequestModel, RegisterUserResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IUrlService _urlService;
        public RegisterUserRequestHandler(ApplicationDbContext context, UserManager<User> userManager, IEmailService emailService, IUrlService urlService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _urlService = urlService;
        }

        public async Task<RegisterUserResponseModel> Handle(RegisterUserRequestModel request,
            CancellationToken cancellationToken)
        {
            var emailUpper = request.Email.ToUpper();
            var isExists = await _context.Users.ActiveAny(p => p.NormalizedEmail == emailUpper);
            if (isExists)
            {
                throw new AlreadyExistsException(nameof(User));
            }
            var teamLeaderId = await _context.Roles.GetByWithSelectAsync(p => p.Name == RoleNames.TeamLeader, p => p.Id,
                cancellationToken: cancellationToken);
            if (string.IsNullOrWhiteSpace(teamLeaderId))
            {
                throw new NotFoundException(nameof(RoleNames));
            }

            request.FullName = request.FullName.Trim();
            var user = new User()
            {
                Email = request.Email,
                FullName = request.FullName,
                NormalizedEmail = emailUpper,
                UserName = request.Email,
                NormalizedUserName = emailUpper,
                UserRoles = new List<UserRole>()
                {
                    new UserRole()
                    {
                        RoleId = teamLeaderId
                    }
                },
                IsEnabled = false,
                Teams = new []
                {
                    new Team()
                    {
                        Name = "Default",
                        MarketPlaces = new List<MarketPlace>()
                        {
                            new()
                            {
                                Name = request.FullName + "'s Marketplace"
                            }
                        }
                    }
                }
            };
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, request.Password);
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendEmailByTemplate(user.Email, EmailTemplateType.AccountVerificationTemplate, new
            {
                username = user.FullName,
                verificationLink = _urlService.GetVerificationUrl(user.Id,token)
            });
            return new RegisterUserResponseModel(user);
        }
    }

    public class RegisterUserResponseModel : UserDto
    {
        public RegisterUserResponseModel(User user) : base(user)
        {
        }
    }
}