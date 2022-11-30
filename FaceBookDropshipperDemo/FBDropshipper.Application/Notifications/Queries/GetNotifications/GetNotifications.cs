using System.Linq.Expressions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Notifications.Models;
using FBDropshipper.Application.Shared;
using FBDropshipper.Common.Enums;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FBDropshipper.Application.Notifications.Queries.GetNotifications
{
    public class GetNotificationsRequestModel : GetPagedRequest<GetNotificationsResponseModel>
    {
        public NotificationStatusTypes Status { get; set; }
        
    }

    public class GetNotificationsRequestModelValidator : PageRequestValidator<GetNotificationsRequestModel>
    {
        public GetNotificationsRequestModelValidator()
        {
            RuleFor(p => p.Status).IsInEnum();
        }
    }

    public class
        GetNotificationsRequestHandler : IRequestHandler<GetNotificationsRequestModel, GetNotificationsResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        public GetNotificationsRequestHandler(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public async Task<GetNotificationsResponseModel> Handle(GetNotificationsRequestModel request,
            CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();
            Expression<Func<UserNotification, bool>> query = p => p.UserId == userId;
            switch (request.Status)
            {
                case NotificationStatusTypes.Read:
                    query = query.AndAlso(p => p.IsRead);    
                    break;
                case NotificationStatusTypes.UnRead:
                    query = query.AndAlso(p => !p.IsRead);    
                    break;
            }

            var list = await _context.UserNotifications.GetManyReadOnly(query, request)
                .Select(NotificationSelector.Selector)
                .ToListAsync(cancellationToken: cancellationToken);
            list.ForEach(p =>
            {
                if (p.Data != null)
                {
                    try
                    {
                        p.Data = JsonConvert.DeserializeObject(p.Data.ToString() ?? string.Empty);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });
            var count = await _context.UserNotifications.ActiveCount(query, cancellationToken);
            return new GetNotificationsResponseModel()
            {
                Data = list,
                Count = count
            };
        }

    }

    public class GetNotificationsResponseModel
    {
        public List<NotificationDto> Data { get; set; }
        public int Count { get; set; }
    }
}