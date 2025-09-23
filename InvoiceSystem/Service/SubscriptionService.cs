using AutoMapper;
using InvoiceSystem.ErrorMessages;
using InvoiceSystem.Exceptions;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvoiceSystem.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SubscriptionService> _logger;

        public SubscriptionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SubscriptionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> CreateSubscriptionAsync(SubscriptionDTO dto)
        {
            try
            {
                // Check if customer has active subscription
                if (await _unitOfWork.Subscriptions.HasActiveSubscriptionAsync(dto.CustomerId, dto.PlanId))
                    throw new BusinessExceptions(AllErrors.SubscriptionAlreadyActive);

                // Check if plan exists
                var plan = await _unitOfWork.Plans.GetByIdAsync(dto.PlanId);
                if (plan == null)
                    throw new NotFoundExceptions(AllErrors.PlanNotFound);

                // Check plan capacity
                var userCount = await _unitOfWork.Subscriptions.CountActiveSubscriptionsForPlanAsync(dto.PlanId);
                if (userCount >= plan.MaxUsers)
                    throw new BusinessExceptions(AllErrors.SubscriptionPlanCapacityExceeded);

                // Create subscription
                var subscription = _mapper.Map<Subscription>(dto);
                subscription.StartDate = DateTime.UtcNow;
                subscription.EndDate = DateTime.UtcNow.AddMonths(3); // Example duration
                subscription.IsActive = true;

                await _unitOfWork.Subscriptions.AddAsync(subscription);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex) when (ex is not BusinessExceptions && ex is not NotFoundExceptions)
            {
                _logger.LogError(ex, "Error creating subscription");
                throw new DatabaseException(AllErrors.InternalServerError, ex);
            }
        }

        public async Task<List<SubscriptionDTO>> GetSubscriptionsByCustomerAsync(int customerId)
        {
            try
            {
                var subs = await _unitOfWork.Subscriptions.GetAllByCustomers(customerId) 
                          .ToListAsync();

                if (!subs.Any())
                    throw new NotFoundExceptions(AllErrors.SubscriptionNotFound);

                return _mapper.Map<List<SubscriptionDTO>>(subs);
            }
            catch (Exception ex) when (ex is not NotFoundExceptions)
            {
                _logger.LogError(ex, "Error fetching subscriptions");
                throw new DatabaseException(AllErrors.InternalServerError, ex);
            }
        }
    }
}
