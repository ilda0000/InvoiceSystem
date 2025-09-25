using AutoMapper;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Repositories;
using InvoiceSystem.Repositories.IRepositories;
using InvoiceSystem.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace InvoiceSystem.Tests.Services
{
    public class InvoiceServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceService> _logger;
        private readonly IEmailService _emailService;
        private readonly InvoiceService _service;

        public InvoiceServiceTests()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _logger = Substitute.For<ILogger<InvoiceService>>();
            _emailService = Substitute.For<IEmailService>();

            _service = new InvoiceService(_unitOfWork, _mapper, _logger, _emailService);
        }

        [Fact]
        public async Task GenerateMonthlyInvoicesMinimalAsync_NoActiveSubscriptions_ReturnsNull()
        {
            // Arrange
            _unitOfWork.Subscriptions.GetAllActive().ToListAsync()
                .Returns(Task.FromResult(new List<Subscription>()));
            // Act
            var result = await _service.GenerateMonthlyInvoicesMinimalAsync();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GenerateMonthlyInvoicesMinimalAsync_WithActiveSubscription_CreatesInvoice()
        {
            // Arrange
            var customer = new Customer { Id = 1, Email = "test@test.com" };
            var plan = new Plan { Id = 1, PricePerMonth = 100m };
            var subscription = new Subscription
            {
                Id = 1,
                CustomerId = 1,
                Customer = customer,
                PlanId = 1,
                Plan = plan,
                StartDate = DateTime.UtcNow.AddMonths(-2),
                EndDate = null
            };

            var subscriptions = new List<Subscription> { subscription };
            _unitOfWork.Subscriptions.GetAllActive()
                .Returns(subscriptions.AsQueryable());

            _unitOfWork.Subscriptions.GetAllByCustomers(customer.Id)
                .Returns(subscriptions.AsQueryable());

            _unitOfWork.Discounts.GetAllAsync()
                .Returns(Task.FromResult(new List<Discount>()));

            _unitOfWork.Invoices.AddAsync(Arg.Any<Invoice>())
                .Returns(Task.CompletedTask);

            _unitOfWork.SaveAsync()
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.GenerateMonthlyInvoicesMinimalAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(subscription.CustomerId, result.CustomerId);
            Assert.Equal(subscription.Id, result.SubscriptionId);
            Assert.Equal(100m, result.TotalAmount);

            await _unitOfWork.Invoices.Received(1).AddAsync(Arg.Any<Invoice>());
            await _unitOfWork.Received(1).SaveAsync();
        }
    }
}
