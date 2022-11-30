using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.UserSubscriptions.Commands.ProcessUserSubscription
{
    public class ProcessUserSubscriptionRequestModel : IRequest<ProcessUserSubscriptionResponseModel>
    {

    }

    public class ProcessUserSubscriptionRequestModelValidator : AbstractValidator<ProcessUserSubscriptionRequestModel>
    {
        public ProcessUserSubscriptionRequestModelValidator()
        {

        }
    }

    public class
        ProcessUserSubscriptionRequestHandler : IRequestHandler<ProcessUserSubscriptionRequestModel,
            ProcessUserSubscriptionResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly IBackgroundTaskQueueService _queueService;
        public ProcessUserSubscriptionRequestHandler(ApplicationDbContext context, IBackgroundTaskQueueService queueService)
        {
            _context = context;
            _queueService = queueService;
        }

        public async Task<ProcessUserSubscriptionResponseModel> Handle(ProcessUserSubscriptionRequestModel request,
            CancellationToken cancellationToken)
        {
            var subscriptions = await _context.UserSubscriptions.GetAll(
                p => (p.CurrentPeriodEnd <= DateTime.UtcNow) && p.CurrentPeriodEnd != null)
                .ToListAsync(cancellationToken: cancellationToken);
            var endedSubs = subscriptions.Where(p => (p.CurrentPeriodEnd <= DateTime.UtcNow) && p.CurrentPeriodEnd != null)
                .ToList();
            if (endedSubs.Any())
            {
                endedSubs.ForEach(p => p.IsActive = false);
                _context.UserSubscriptions.UpdateRange(endedSubs);
                await _context.SaveChangesAsync(cancellationToken);
            }
            
            return new ProcessUserSubscriptionResponseModel();
        }

    }

    public class ProcessUserSubscriptionResponseModel
    {

    }
}