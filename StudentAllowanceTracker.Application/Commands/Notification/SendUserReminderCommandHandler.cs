using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Application.DTOs;
using AutoMapper;
using StudentAllowanceTracker.Shared.Helpers;

namespace StudentAllowanceTracker.Application.Commands.Notification
{
    public class SendUserReminderCommandHandler : IRequestHandler<SendUserReminderCommand, Result<List<UserSubscriptionDTO>>>
    {
        private readonly IEmailService _emailService;
        private readonly IBaseRepository<UserSubscription> _userSubscriptionRepository;
        private readonly IMapper _mapper;

        public SendUserReminderCommandHandler(
            IEmailService emailService,
            IBaseRepository<UserSubscription> userSubscriptionRepository,
            IMapper mapper)
        {
            _emailService = emailService;
            _userSubscriptionRepository = userSubscriptionRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<UserSubscriptionDTO>>> Handle(SendUserReminderCommand command, CancellationToken cancellationToken)
        {
            var query = string.IsNullOrEmpty(command.UserId)
                ? _userSubscriptionRepository.GetQueryable().Include(u => u.User).Where(u => u.ReceiveEmail)
                : _userSubscriptionRepository.GetQueryable().Include(u => u.User).Where(u => u.UserId == command.UserId && u.ReceiveEmail);

            var subscriptions = await query.ToListAsync(cancellationToken);

            if (!subscriptions.Any())
                return Result<List<UserSubscriptionDTO>>.Ok(new List<UserSubscriptionDTO>());

            var now = TimeHelper.Now(); 
            var subscriptionsToSend = subscriptions
                .Where(sub =>
                    (sub.Frequency == ReminderFrequency.Daily && sub.LastReminderSentAt < now.Date) ||
                    (sub.Frequency == ReminderFrequency.Weekly && sub.LastReminderSentAt <= now.Date.AddDays(-7))
                )
                .ToList();

            if (!subscriptionsToSend.Any())
                return Result<List<UserSubscriptionDTO>>.Ok(new List<UserSubscriptionDTO>());

            foreach (var sub in subscriptionsToSend)
            {
                var emailBody = $"Hi {sub.User.FirstName},<br/><br/>Don't forget to log your expenses today!";
                await _emailService.SendEmailAsync(sub.User.Email, "Daily Reminder", emailBody);
                sub.LastReminderSentAt = now;
                await _userSubscriptionRepository.UpdateAsync(sub);
            }

            var dtoList = _mapper.Map<List<UserSubscriptionDTO>>(subscriptionsToSend);
            return Result<List<UserSubscriptionDTO>>.Ok(dtoList);
        }
    }
}