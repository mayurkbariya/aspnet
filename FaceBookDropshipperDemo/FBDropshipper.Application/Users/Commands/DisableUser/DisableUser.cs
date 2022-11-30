using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Users.Commands.DisableUser
{
    public class DisableUserRequestModel : IRequest<DisableUserResponseModel>
    {
        public string Id { get; set; }

    }

    public class DisableUserRequestModelValidator : AbstractValidator<DisableUserRequestModel>
    {
        public DisableUserRequestModelValidator()
        {
            RuleFor(p => p.Id).Required();
        }
    }

    public class
        DisableUserRequestHandler : IRequestHandler<DisableUserRequestModel, DisableUserResponseModel>
    {
        private readonly ApplicationDbContext _context;

        public DisableUserRequestHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DisableUserResponseModel> Handle(DisableUserRequestModel request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users.GetByReadOnlyAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);
            if (user == null)
            {
                throw new NotFoundException(nameof(user));
            }
            user.IsEnabled = false;
            _context.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            return new DisableUserResponseModel();

        }

    }

    public class DisableUserResponseModel
    {

    }
}