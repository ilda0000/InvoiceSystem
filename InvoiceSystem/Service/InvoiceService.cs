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
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<InvoiceDTO?> GenerateMonthlyInvoicesMinimalAsync()
        {
            try
            {
                var billingDate = DateTime.UtcNow;

              
                var activeSubs = await _unitOfWork.Subscriptions
                    .GetAllActive()
                    .Include(s => s.Plan)
                    .Include(s => s.Customer)
                    .ToListAsync();

                if (activeSubs == null || activeSubs.Count == 0)
                {
                    _logger.LogInformation("No active subscriptions found for billing date: {BillingDate}", billingDate);
                    return null;
                }

                var allDiscounts = await _unitOfWork.Discounts.GetAllAsync();

                foreach (var sub in activeSubs)
                {
                    var pastSubs = await _unitOfWork.Subscriptions
                    .GetAllByCustomers(sub.CustomerId)
                     .ToListAsync(); 

                    int totalMonths = pastSubs.Sum(s =>
                        s.EndDate.HasValue
                            ? (s.EndDate.Value - s.StartDate).Days / 30
                            : 0);

                    var plan = sub.Plan;

                    decimal basePrice = plan.PricePerMonth;
                    decimal totalDiscount = 0m;
                    int? discountId = null;

                    // Loyalty discount
                    var loyalty = allDiscounts
                        .Where(d => d.MinMonthsRequired > 0 && totalMonths >= d.MinMonthsRequired)
                        .OrderByDescending(d => d.MinMonthsRequired)
                        .FirstOrDefault();

                    if (loyalty != null)
                    {
                        totalDiscount += loyalty.Type == "Percentage"
                            ? basePrice * loyalty.Value / 100m
                            : loyalty.Value;

                        discountId = loyalty.Id;
                    }

                    // Payment method discount
                    var paymentMethodDiscount = allDiscounts
                        .FirstOrDefault(d => d.MinMonthsRequired == 0 && d.Name == "Credit Card");

                    if (paymentMethodDiscount != null)
                    {
                        totalDiscount += paymentMethodDiscount.Type == "Percentage"
                            ? basePrice * paymentMethodDiscount.Value / 100m
                            : paymentMethodDiscount.Value;
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

                    try
                    {
                        await _unitOfWork.Invoices.AddAsync(invoice);
                        await _unitOfWork.SaveAsync();
                    }
                    catch (Exception ex)
                    {
                        throw new DatabaseException(AllErrors.InvoiceCreationFailed, ex);
                    }

                    _logger.LogInformation("Invoice created: InvoiceId: {InvoiceId} for CustomerId: {CustomerId}",
                        invoice.Id, sub.CustomerId);

                    try
                    {
                        await _emailService.SendInvoiceEmailAsync(sub.Customer.Email, invoice);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex,
                            "Failed sending invoice email. InvoiceId={InvoiceId} CustomerId={CustomerId}",
                            invoice.Id, sub.CustomerId);
                    }

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

                _logger.LogInformation("No invoices generated for billing date: {BillingDate}", billingDate);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating monthly invoices.");
                throw;
            }
        }

        public async Task<List<InvoiceDTO>> GetInvoicesByCustomerAsync(int customerId)
        {
            var invoices = await _unitOfWork.Invoices.GetByCustomerIdAsync(customerId);
            return _mapper.Map<List<InvoiceDTO>>(invoices);
        }

        public async Task<decimal> CalculateDiscountedAmountAsync(int customerId, Plan plan)
        {
            int totalMonths = await _unitOfWork.Subscriptions
                .GetAllByCustomers(customerId)
                .SumAsync(s => s.EndDate.HasValue
                    ? (s.EndDate.Value - s.StartDate).Days / 30
                    : 0);

            var discount = await _unitOfWork.Discounts.GetBestApplicableDiscountAsync(totalMonths);

            var baseAmount = plan.PricePerMonth;
            var discountValue = 0m;

            if (discount != null)
            {
                discountValue = discount.Type == "Percentage"
                    ? baseAmount * discount.Value / 100m
                    : discount.Value;
            }

            return baseAmount - discountValue;
        }
    }
}
