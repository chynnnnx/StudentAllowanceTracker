using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Application.Queries.Notification
{
    public class GetCurrentSubscriptionQueryHandler : IRequestHandler<GetCurrentSubscriptionQuery, Result<List<UserSubscriptionDTO>>>
    {
        private readonly IBaseRepository<UserSubscription> _userSubscriptionRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public GetCurrentSubscriptionQueryHandler(
            IBaseRepository<UserSubscription> userSubscriptionRepository,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Result<List<UserSubscriptionDTO>>> Handle(GetCurrentSubscriptionQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (string.IsNullOrEmpty(userId))
                return Result<List<UserSubscriptionDTO>>.Fail(ResultStatus.Unauthorized, "User not authenticated");

            var subscription = await _userSubscriptionRepository.FindOneAsync(s => s.UserId == userId);

            var subscriptions = new List<UserSubscription>();

            if (subscription != null)
            {
                subscriptions.Add(subscription);
            }
            else
            {
                var newSubscription = new UserSubscription
                {
                    UserId = userId,
                    ReceiveEmail = false, 
                    Frequency = ReminderFrequency.Daily,
                    LastReminderSentAt = DateTime.UtcNow.AddDays(-1)
                };

                await _userSubscriptionRepository.AddAsync(newSubscription);
                subscriptions.Add(newSubscription);
            }

            var dtoList = _mapper.Map<List<UserSubscriptionDTO>>(subscriptions);
            return Result<List<UserSubscriptionDTO>>.Ok(dtoList);
        }
    }
}