using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FBDropshipper.Application.Users.Commands.ResetPassword;

public class ResetPasswordRequestModel : IRequest<ResetPasswordResponseModel>
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

public class ResetPasswordRequestModelValidator : AbstractValidator<ResetPasswordRequestModel>
{
    public ResetPasswordRequestModelValidator()
    {
        RuleFor(p => p.UserId).Required();
        RuleFor(p => p.Token).Required();
        RuleFor(p => p.Password).Password();
        RuleFor(p => p.ConfirmPassword).Matches(p => p.Password);
    }
}

public class
    ResetPasswordRequestHandler : IRequestHandler<ResetPasswordRequestModel, ResetPasswordResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    public ResetPasswordRequestHandler(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<ResetPasswordResponseModel> Handle(ResetPasswordRequestModel request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users.GetByAsync(p => p.Id == request.UserId, cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new NotFoundException(nameof(user));
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (result.Succeeded)
        {
            return new ResetPasswordResponseModel();
        }
        throw new BadRequestException(result.Errors.Select(p => p.Description).ToList());
    }

}

public class ResetPasswordResponseModel
{

}