using AutoMapper;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Repositories.IRepositories;
using Microsoft.Extensions.Logging;

namespace InvoiceSystem.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PaymentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaymentDTO?> AutoRegisterPaymentAsync()
        {
            int customerId = 4; // checked for id manually or payments

            _logger.LogInformation("Starting auto-payment registration for CustomerId {CustomerId}", customerId);

            var subscriptions = await _unitOfWork.Subscriptions.GetByCustomerIdAsync(customerId);
            var activeSub = subscriptions.FirstOrDefault(s => s.IsActive);

            if (activeSub == null)
            {
                _logger.LogWarning("No active subscription found for CustomerId {CustomerId}", customerId);
                return null;
            }

            var unpaidInvoice = (await _unitOfWork.Invoices.GetByCustomerIdAsync(customerId))
                .FirstOrDefault(i => i.SubscriptionId == activeSub.Id && !i.Paid);

            if (unpaidInvoice == null)
            {
                _logger.LogWarning("No unpaid invoice found for active subscription {SubscriptionId}", activeSub.Id);
                return null;
            }

            var paymentMethods = await _unitOfWork.PaymentMethods.GetAllAsync();
            var method = paymentMethods.FirstOrDefault(m => m.IsActive);

            if (method == null)
            {
                _logger.LogWarning("No active payment method found");
                return null;
            }

            var payment = new Payment
            {
                InvoiceId = unpaidInvoice.Id,
                PaymentDate = DateTime.UtcNow,
                AmountPaid = unpaidInvoice.TotalAmount,
                PaymentMethodId = method.Id
            };

            await _unitOfWork.Payments.AddAsync(payment);

            unpaidInvoice.Paid = true;
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Payment recorded for InvoiceId {InvoiceId} with Amount {Amount}",
                unpaidInvoice.Id, unpaidInvoice.TotalAmount);

            return _mapper.Map<PaymentDTO>(payment);
        }
    }
}
