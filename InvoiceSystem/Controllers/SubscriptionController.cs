using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Validators;
using InvoiceSystem.Service;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace InvoiceSystem.Controllers
{
    [ApiController]
    [Route("api/subscriptions")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IValidator<SubscriptionDTO> _validator;

        public SubscriptionController(ISubscriptionService subscriptionService, IValidator<SubscriptionDTO> validator)
        {
            _subscriptionService = subscriptionService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] SubscriptionReqDTOcs dto)
        {
            var subscriptionDto = new SubscriptionDTO
            {
                CustomerId = dto.CustomerId,
                PlanId = dto.PlanId
                // StartDate and IsActive will be set by the service
            };

            // Trigger validation
            await _validator.ValidateAndThrowAsync(subscriptionDto);

            var result = await _subscriptionService.CreateSubscriptionAsync(subscriptionDto);
            if (!result)
                return BadRequest("Subscription could not be created.");

            return Ok("Subscription created successfully.");
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetSubscriptionsByCustomer(int customerId)
        {
            var subs = await _subscriptionService.GetSubscriptionsByCustomerAsync(customerId);
            return Ok(subs);
        }
    }
}
