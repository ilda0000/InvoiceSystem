using AutoMapper;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Repositories.IRepositories;

namespace InvoiceSystem.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubscriptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateSubscriptionAsync(SubscriptionDTO dto)
        {
           //If plan is active
            if (await _unitOfWork.Subscriptions.HasActiveSubscriptionAsync(dto.CustomerId, dto.PlanId))
                throw new Exception("Customer already has an active subscription for this plan.");

            //If plan exists
            var plan = await _unitOfWork.Plans.GetByIdAsync(dto.PlanId);
            if (plan == null)
                throw new Exception("Plan not found.");

           //If there are more users 
            var userCount = await _unitOfWork.Subscriptions.CountActiveSubscriptionsForPlanAsync(dto.PlanId);
            if (userCount >= plan.MaxUsers)
                throw new Exception("Plan capacity exceeded. Max 5 users allowed.");

            //Plan is active
            var subscription = _mapper.Map<Subscription>(dto);
            subscription.StartDate = DateTime.UtcNow;
            subscription.EndDate = DateTime.UtcNow.AddMonths(3);
            subscription.IsActive = true;

            await _unitOfWork.Subscriptions.AddAsync(subscription);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<List<SubscriptionDTO>> GetSubscriptionsByCustomerAsync(int customerId)
        {
            var subs = await _unitOfWork.Subscriptions.GetAllByCustomerAsync(customerId);
            return _mapper.Map<List<SubscriptionDTO>>(subs);
        }
    }
}
