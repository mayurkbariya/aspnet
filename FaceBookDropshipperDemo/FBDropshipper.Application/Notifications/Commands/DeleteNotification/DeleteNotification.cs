using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Notifications.Commands.DeleteNotification
{
    public class DeleteNotificationRequestModel : IRequest<DeleteNotificationResponseModel>
    {
        public int Id { get; set; }
    }

    public class DeleteNotificationRequestModelValidator : AbstractValidator<DeleteNotificationRequestModel>
    {
        public DeleteNotificationRequestModelValidator()
        {
            RuleFor(p => p.Id).Required();
        }
    }

    public class
        DeleteNotificationRequestHandler : IRequestHandler<DeleteNotificationRequestModel,
            DeleteNotificationResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        public DeleteNotificationRequestHandler(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public async Task<DeleteNotificationResponseModel> Handle(DeleteNotificationRequestModel request,
            CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();
            var notification = await _context.UserNotifications.GetByAsync(p => p.Id == request.Id && p.UserId == userId, cancellationToken: cancellationToken);
            if (notification == null)
            {
                throw new NotFoundException(nameof(notification));
            }
            _context.UserNotifications.Remove(notification);
            await _context.SaveChangesAsync(cancellationToken);
            return new DeleteNotificationResponseModel();
        }

    }

    public class DeleteNotificationResponseModel
    {

    }
}