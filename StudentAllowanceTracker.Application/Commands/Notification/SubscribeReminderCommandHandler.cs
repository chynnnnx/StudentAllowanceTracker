using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Application.Commands.Notification
{
    public class SubscribeReminderCommandHandler : IRequestHandler<SubscribeReminderCommand, Result<UserSubscriptionDTO>>
    {
        private readonly IBaseRepository<UserSubscription> _userSubscriptionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public SubscribeReminderCommandHandler(
            IBaseRepository<UserSubscription> userSubscriptionRepository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<UserSubscriptionDTO>> Handle(SubscribeReminderCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (string.IsNullOrEmpty(userId))
                return Result<UserSubscriptionDTO>.Fail(ResultStatus.Unauthorized, "User not authenticated");

            var existing = await _userSubscriptionRepository.FindOneAsync(s => s.UserId == userId);

            UserSubscription subscription;

            if (existing != null)
            {
                existing.ReceiveEmail = command.ReceiveEmail;
                existing.Frequency = command.Frequency;

                await _userSubscriptionRepository.UpdateAsync(existing);
                subscription = existing;
            }
            else
            {
               
                subscription = new UserSubscription
                {
                    UserId = userId,
                    ReceiveEmail = command.ReceiveEmail,
                    Frequency = command.Frequency,
                    LastReminderSentAt = DateTime.UtcNow.AddDays(-1) 
                };

                await _userSubscriptionRepository.AddAsync(subscription);
            }


            var dto = _mapper.Map<UserSubscriptionDTO>(subscription);
            return Result<UserSubscriptionDTO>.Ok(dto);
        }
    }
}