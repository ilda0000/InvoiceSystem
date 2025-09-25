using AutoMapper;
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
            // Check if customer has active subscription
            if (await _unitOfWork.Subscriptions.HasActiveSubscriptionAsync(dto.CustomerId, dto.PlanId))
                throw new BusinessExceptions("Customer already has an active subscription.");

            // Check if plan exists
            var plan = await _unitOfWork.Plans.GetByIdAsync(dto.PlanId);
            if (plan == null)
                throw new NotFoundExceptions("Plan not found.");

            // Check plan capacity
            var userCount = await _unitOfWork.Subscriptions.CountActiveSubscriptionsForPlanAsync(dto.PlanId);
            if (userCount >= plan.MaxUsers)
                throw new BusinessExceptions("Subscription plan capacity exceeded.");

            // Create subscription
            var subscription = _mapper.Map<Subscription>(dto);
            subscription.StartDate = DateTime.UtcNow;
            subscription.EndDate = DateTime.UtcNow.AddMonths(3);
            subscription.IsActive = true;

            await _unitOfWork.Subscriptions.AddAsync(subscription);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Subscription created for CustomerId={CustomerId}", dto.CustomerId);
            return true;
        }

        public async Task<List<SubscriptionDTO>> GetSubscriptionsByCustomerAsync(int customerId)
        {
            var subs = await _unitOfWork.Subscriptions.GetAllByCustomers(customerId).ToListAsync();

            if (!subs.Any())
                throw new NotFoundExceptions("No subscriptions found for this customer.");

            return _mapper.Map<List<SubscriptionDTO>>(subs);
        }
    }
}
