using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FBDropshipper.Application.Users.Commands.VerifyUser;

public class VerifyUserRequestModel : IRequest<VerifyUserResponseModel>
{
    public string UserId { get; set; }
    public string Token { get; set; }
}

public class VerifyUserRequestModelValidator : AbstractValidator<VerifyUserRequestModel>
{
    public VerifyUserRequestModelValidator()
    {
        RuleFor(p => p.UserId).Required();
        RuleFor(p => p.Token).Required();
    }
}

public class
    VerifyUserRequestHandler : IRequestHandler<VerifyUserRequestModel, VerifyUserResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    public VerifyUserRequestHandler(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<VerifyUserResponseModel> Handle(VerifyUserRequestModel request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users.GetByAsync(p => p.Id == request.UserId, cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new NotFoundException(nameof(user));
        }

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        if (result.Succeeded)
        {
            user.IsEnabled = true;
            _context.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            return new VerifyUserResponseModel();
        }
        throw new BadRequestException(result.Errors.Select(p => p.Description).ToList());
    }

}

public class VerifyUserResponseModel
{

}