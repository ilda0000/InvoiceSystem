using AutoMapper;
using InvoiceSystem.ErrorMessages;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
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
            int customerId = 4; // Example customer
            _logger.LogInformation("Starting auto-payment registration for CustomerId {CustomerId}", customerId);

            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
            if (customer == null)
            {
                _logger.LogWarning(AllErrors.CustomerNotFound);
                return null;
            }

            var subscriptions = await _unitOfWork.Subscriptions.GetAllByCustomers(customerId).ToListAsync();
            var activeSub = subscriptions.FirstOrDefault(s => s.IsActive);

            if (activeSub == null)
            {
                _logger.LogWarning("No active subscription found for CustomerId {CustomerId}", customerId);
                return null;
            }

            var unpaidInvoice = (await _unitOfWork.Invoices.GetByCustomerIdAsync(customerId))
                .FirstOrDefault(i => i.SubscriptionId == activeSub.Id && i.Status == Invoice.InvoiceStatus.NotPaid);

            if (unpaidInvoice == null)
            {
                _logger.LogWarning(AllErrors.PaymentInvoiceNotFound);
                return null;
            }

            var paymentMethods = await _unitOfWork.PaymentMethods.GetAllAsync();
            var method = paymentMethods.FirstOrDefault(m => m.IsActive);

            if (method == null)
            {
                _logger.LogWarning(AllErrors.PaymentMethodInactive);
                return null;
            }

            if (unpaidInvoice.TotalAmount <= 0)
            {
                _logger.LogWarning(AllErrors.PaymentAmountInvalid);
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

            unpaidInvoice.Status = Invoice.InvoiceStatus.Paid;

            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Payment recorded for InvoiceId {InvoiceId} with Amount {Amount}",
                unpaidInvoice.Id, unpaidInvoice.TotalAmount);

            return _mapper.Map<PaymentDTO>(payment);
        }
    }
}
