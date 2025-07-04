using AutoMapper;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Repositories.IRepositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceSystem.Service
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceService> _logger;
        private readonly IEmailService _emailService;

        public InvoiceService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<InvoiceService> logger,
            IEmailService emailService 
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<InvoiceDTO?> GenerateMonthlyInvoicesMinimalAsync()
        {
            var activeSubs = await _unitOfWork.Subscriptions.GetAllActiveAsync();
            var billingDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            foreach (var sub in activeSubs)
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(sub.CustomerId);
                if (customer == null)
                {
                    _logger.LogWarning("Customer not found for subscription {SubscriptionId}", sub.Id);
                    continue;
                }

                var plan = await _unitOfWork.Plans.GetByIdAsync(sub.PlanId);
                if (plan == null)
                {
                    _logger.LogWarning("Plan not found for subscription {SubscriptionId} (PlanId: {PlanId})", sub.Id, sub.PlanId);
                    continue;
                }

                if (await _unitOfWork.Invoices.InvoiceExistsAsync(sub.Id, billingDate))
                {
                    _logger.LogInformation("Invoice already exists for subscription {SubscriptionId} and date {BillingDate}", sub.Id, billingDate);
                    continue;
                }

                // Total months the customer has been subscribed
                var pastSubs = await _unitOfWork.Subscriptions.GetAllByCustomerAsync(sub.CustomerId);
                int totalMonths = pastSubs.Sum(s =>
                    s.EndDate.HasValue ? (s.EndDate.Value - s.StartDate).Days / 30 : 0);

                decimal basePrice = plan.PricePerMonth;
                decimal totalDiscount = 0;
                int? discountId = null;

                // Loyalty discount
                var allDiscounts = await _unitOfWork.Discounts.GetAllAsync();
                var loyalty = allDiscounts
                    .Where(d => d.MinMonthsRequired > 0 && totalMonths >= d.MinMonthsRequired)
                    .OrderByDescending(d => d.MinMonthsRequired)
                    .FirstOrDefault();

                if (loyalty != null)
                {
                    totalDiscount += loyalty.Type == "Percentage"
                        ? basePrice * loyalty.Value / 100
                        : loyalty.Value;

                    discountId = loyalty.Id;

                    _logger.LogInformation("Loyalty discount applied: {Discount} for CustomerId: {CustomerId}", totalDiscount, sub.CustomerId);
                }

                // Payment method discount – hardcoded to "Credit Card"
                var paymentMethodDiscount = allDiscounts.FirstOrDefault(d => d.MinMonthsRequired == 0 && d.Name == "Credit Card");

                if (paymentMethodDiscount != null)
                {
                    totalDiscount += paymentMethodDiscount.Type == "Percentage"
                        ? basePrice * paymentMethodDiscount.Value / 100
                        : paymentMethodDiscount.Value;

                    _logger.LogInformation("Payment method discount applied: {Discount} for CustomerId: {CustomerId}", paymentMethodDiscount.Value, sub.CustomerId);
                }

                decimal total = basePrice - totalDiscount;

                var invoice = new Invoice
                {
                    SubscriptionId = sub.Id,
                    BillingDate = billingDate,
                    TotalAmount = total,
                    Paid = false,
                    DiscountId = discountId
                };

                await _unitOfWork.Invoices.AddAsync(invoice);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Invoice created: InvoiceId: {InvoiceId} for CustomerId: {CustomerId}", invoice.Id, sub.CustomerId);

                await _emailService.SendInvoiceEmailAsync(customer.Email, invoice);

                return new InvoiceDTO
                {
                    Id = invoice.Id,
                    CustomerId = sub.CustomerId,
                    SubscriptionId = sub.Id,
                    BillingDate = billingDate,
                    TotalAmount = total,
                    DiscountApplied = totalDiscount,
                    Paid = false,
                    Status = "created"
                };
            }

            _logger.LogInformation("No active subscriptions found for billing date: {BillingDate}", billingDate);
            return null;
        }

        public async Task<List<InvoiceDTO>> GetInvoicesByCustomerAsync(int customerId)
        {
            var invoices = await _unitOfWork.Invoices.GetByCustomerIdAsync(customerId);
            return _mapper.Map<List<InvoiceDTO>>(invoices);
        }

        public async Task<decimal> CalculateDiscountedAmountAsync(int customerId, Plan plan)
        {
            var subs = await _unitOfWork.Subscriptions.GetByCustomerIdAsync(customerId);
            int totalMonths = subs.Sum(s => s.EndDate.HasValue ? (s.EndDate.Value - s.StartDate).Days / 30 : 0);

            var discount = await _unitOfWork.Discounts.GetBestApplicableDiscountAsync(totalMonths);

            decimal baseAmount = plan.PricePerMonth;
            decimal discountValue = 0;

            if (discount != null)
            {
                discountValue = discount.Type == "Percentage"
                    ? baseAmount * discount.Value / 100
                    : discount.Value;
            }

            decimal totalAmount = baseAmount - discountValue;
            return totalAmount;
        }
    }
}