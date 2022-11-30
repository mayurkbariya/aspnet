using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.Notifications.Commands.ReadAllNotification
{
    public class ReadAllNotificationRequestModel : IRequest<ReadAllNotificationResponseModel>
    {
    }

    public class ReadAllNotificationRequestModelValidator : AbstractValidator<ReadAllNotificationRequestModel>
    {
        public ReadAllNotificationRequestModelValidator()
        {

        }
    }

    public class
        ReadAllNotificationRequestHandler : IRequestHandler<ReadAllNotificationRequestModel,
            ReadAllNotificationResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        public ReadAllNotificationRequestHandler(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public async Task<ReadAllNotificationResponseModel> Handle(ReadAllNotificationRequestModel request,
            CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();
            var notifications = await _context.UserNotifications.GetAll(p => p.UserId == userId && !p.IsRead)
                .ToListAsync(cancellationToken: cancellationToken);
            if (notifications.Any())
            {
                notifications.ForEach(p =>
                {
                    p.IsRead = true;
                    p.ReadDate = DateTime.UtcNow;
                });
                _context.UserNotifications.UpdateRange(notifications);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return new ReadAllNotificationResponseModel();
        }

    }

    public class ReadAllNotificationResponseModel
    {

    }
}