using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Repositories.IRepositories;
using InvoiceSystem.Service;
using NSubstitute;
using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Tests.Services
{
    public class SubscriptionServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SubscriptionService> _logger;
        private readonly SubscriptionService _service;

        public SubscriptionServiceTests()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _logger = Substitute.For<ILogger<SubscriptionService>>();
            _service = new SubscriptionService(_unitOfWork, _mapper, _logger);
        }

        [Fact]
        public async Task CreateSubscriptionAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var dto = new SubscriptionDTO { CustomerId = 1, PlanId = 1 };
            var plan = new Plan { Id = 1, MaxUsers = 10 };

            _unitOfWork.Subscriptions.HasActiveSubscriptionAsync(dto.CustomerId, dto.PlanId)
                .Returns(Task.FromResult(false));

            _unitOfWork.Plans.GetByIdAsync(dto.PlanId).Returns(Task.FromResult(plan));
            _unitOfWork.Subscriptions.CountActiveSubscriptionsForPlanAsync(dto.PlanId).Returns(Task.FromResult(0));

            _mapper.Map<Subscription>(dto).Returns(new Subscription { CustomerId = dto.CustomerId, PlanId = dto.PlanId });
            _unitOfWork.Subscriptions.AddAsync(Arg.Any<Subscription>()).Returns(Task.CompletedTask);
            _unitOfWork.SaveAsync().Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateSubscriptionAsync(dto);

            // Assert
            Assert.True(result);
            await _unitOfWork.Subscriptions.Received(1).AddAsync(Arg.Any<Subscription>());
            await _unitOfWork.Received(1).SaveAsync();
        }

        [Fact]
        public async Task CreateSubscriptionAsync_WhenPlanNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var dto = new SubscriptionDTO { CustomerId = 1, PlanId = 99 };

            _unitOfWork.Subscriptions.HasActiveSubscriptionAsync(dto.CustomerId, dto.PlanId)
                .Returns(Task.FromResult(false));
            _unitOfWork.Plans.GetByIdAsync(dto.PlanId).Returns(Task.FromResult<Plan>(null));

            // Act & Assert
            await Assert.ThrowsAsync<InvoiceSystem.Exceptions.NotFoundExceptions>(() =>
                _service.CreateSubscriptionAsync(dto));
        }
    }
}
   