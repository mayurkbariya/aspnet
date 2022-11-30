using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Templates;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FBDropshipper.Application.Users.Commands.ForgetPassword;

public class ForgetPasswordRequestModel : IRequest<ForgetPasswordResponseModel>
{
    public string Email { get; set; }
}

public class ForgetPasswordRequestModelValidator : AbstractValidator<ForgetPasswordRequestModel>
{
    public ForgetPasswordRequestModelValidator()
    {
        RuleFor(p => p.Email).EmailAddress();
    }
}

public class
    ForgetPasswordRequestHandler : IRequestHandler<ForgetPasswordRequestModel, ForgetPasswordResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;
    private readonly IUrlService _urlService;
    public ForgetPasswordRequestHandler(ApplicationDbContext context, UserManager<User> userManager, IEmailService emailService, IUrlService urlService)
    {
        _context = context;
        _userManager = userManager;
        _emailService = emailService;
        _urlService = urlService;
    }

    public async Task<ForgetPasswordResponseModel> Handle(ForgetPasswordRequestModel request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users.GetByAsync(p => p.Email.ToLower()
                                                  == request.Email.ToLower(), cancellationToken: cancellationToken);
        if (user == null)
        {
            return new ForgetPasswordResponseModel();
        }

        if (!user.IsEnabled)
        {
            return new ForgetPasswordResponseModel();
        }
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _emailService.SendEmailByTemplate(user.Email, EmailTemplateType.PasswordResetTemplate, new
        {
            username = user.FullName,
            resetPasswordLink = _urlService.GeneratePasswordResetUrl(user.Id,token),
        });
        return new ForgetPasswordResponseModel();
    }

}

public class ForgetPasswordResponseModel
{

}