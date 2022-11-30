using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Users.Commands.EnableUser
{
    public class EnableUserRequestModel : IRequest<EnableUserResponseModel>
    {
        public string Id { get; set; }
    }

    public class EnableUserRequestModelValidator : AbstractValidator<EnableUserRequestModel>
    {
        public EnableUserRequestModelValidator()
        {
            RuleFor(p => p.Id).Required();
        }
    }

    public class
        EnableUserRequestHandler : IRequestHandler<EnableUserRequestModel, EnableUserResponseModel>
    {
        private readonly ApplicationDbContext _context;

        public EnableUserRequestHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EnableUserResponseModel> Handle(EnableUserRequestModel request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users.GetByReadOnlyAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);
            if (user == null)
            {
                throw new NotFoundException(nameof(user));
            }

            user.IsEnabled = true;
            _context.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            return new EnableUserResponseModel();
        }
    }

    public class EnableUserResponseModel
    {

    }
}