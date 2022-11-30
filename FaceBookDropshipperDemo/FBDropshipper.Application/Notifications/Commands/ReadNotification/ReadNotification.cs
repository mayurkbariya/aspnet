using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Notifications.Models;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Notifications.Commands.ReadNotification
{
    public class ReadNotificationRequestModel : IRequest<ReadNotificationResponseModel>
    {
        public int Id { get; set; }
    }

    public class ReadNotificationRequestModelValidator : AbstractValidator<ReadNotificationRequestModel>
    {
        public ReadNotificationRequestModelValidator()
        {
            RuleFor(p => p.Id).Required();
        }
    }

    public class
        ReadNotificationRequestHandler : IRequestHandler<ReadNotificationRequestModel, ReadNotificationResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        public ReadNotificationRequestHandler(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public async Task<ReadNotificationResponseModel> Handle(ReadNotificationRequestModel request,
            CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();
            var notification =
                await _context.UserNotifications.GetByAsync(p => p.Id == request.Id && p.UserId == userId, cancellationToken: cancellationToken);
            if (notification == null)
            {
                throw new NotFoundException(nameof(notification));
            }

            if (notification.IsRead)
            {
                return new ReadNotificationResponseModel(notification);
            }
            notification.IsRead = true;
            notification.ReadDate = DateTime.UtcNow;
            _context.UserNotifications.Update(notification);
            await _context.SaveChangesAsync(cancellationToken);

            return new ReadNotificationResponseModel(notification);
        }

    }

    public class ReadNotificationResponseModel : NotificationDto
    {
        public ReadNotificationResponseModel(UserNotification notification) : base(notification)
        {
        }
    }
}