using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Common.Constants;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FBDropshipper.Application.Users.Commands.ChangePassword
{
    public class ChangePasswordRequestModel : IRequest<ChangePasswordResponseModel>
    {
        public string PreviousPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordRequestModelValidator : AbstractValidator<ChangePasswordRequestModel>
    {
        public ChangePasswordRequestModelValidator()
        {
            RuleFor(p => p.PreviousPassword).Required();
            RuleFor(p => p.NewPassword).Required();
            RuleFor(p => p.ConfirmPassword).Matches(p => p.NewPassword);
        }
    }

    public class
        ChangePasswordRequestHandler : IRequestHandler<ChangePasswordRequestModel, ChangePasswordResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        public ChangePasswordRequestHandler(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public async Task<ChangePasswordResponseModel> Handle(ChangePasswordRequestModel request,
            CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();
            var user = await _context.Users.GetByAsync(p => p.Id == userId, cancellationToken: cancellationToken);
            if (user == null)
            {
                throw new NotFoundException(nameof(user));
            }

            var passwordHasher = new PasswordHasher<User>();
            if (passwordHasher.VerifyHashedPassword(user,user.PasswordHash,request.PreviousPassword) == PasswordVerificationResult.Failed)
            {
                throw new CannotUpdateException(Messages.InvalidPassword);
            }
            user.PasswordHash = passwordHasher.HashPassword(user, request.NewPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            return new ChangePasswordResponseModel();
        }

    }

    public class ChangePasswordResponseModel
    {

    }
}