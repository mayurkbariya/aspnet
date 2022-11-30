using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Notifications.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;

namespace FBDropshipper.Application.Notifications.Commands.CreateNotification
{
    public class CreateNotificationRequestModel : IRequest<CreateNotificationResponseModel>
    {
        public NotificationType Type { get; set; }
        public string[] UserIds { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public class CreateNotificationRequestModelValidator : AbstractValidator<CreateNotificationRequestModel>
    {
        public CreateNotificationRequestModelValidator()
        {
            RuleFor(p => p.UserIds).Required();
            RuleFor(p => p.Type).IsInEnum();
            RuleFor(p => p.Message).Required().Max(255);
        }
    }

    public class
        CreateNotificationRequestHandler : IRequestHandler<CreateNotificationRequestModel,
            CreateNotificationResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly IAlertService _alertService;
        public CreateNotificationRequestHandler(ApplicationDbContext context, IAlertService alertService)
        {
            _context = context;
            _alertService = alertService;
        }

        public async Task<CreateNotificationResponseModel> Handle(CreateNotificationRequestModel request,
            CancellationToken cancellationToken)
        {
            foreach (var userId in request.UserIds)
            {
                var notification = new UserNotification()
                {
                    Message = request.Message,
                    UserId = userId,
                    Type = request.Type.ToInt(),
                    IsRead = false,
                    Data = request.Data != null ? JsonConvert.SerializeObject(request.Data) : null,
                };
                _context.UserNotifications.Add(notification);
            }
            await _context.SaveChangesAsync(cancellationToken);
            foreach (var userId in request.UserIds)
            {
                await _alertService.SendNotificationToUser(userId, request.Message,request.Type,request.Data);
            }
            return new CreateNotificationResponseModel();
        }

    }

    public class CreateNotificationResponseModel
    {
        
    }
}