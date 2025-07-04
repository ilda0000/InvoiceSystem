using InvoiceSystem.Models.DTO;
using InvoiceSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceSystem.Controllers
{
    [ApiController]
    [Route("api/subscriptions")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] SubscriptionReqDTOcs dto)
        {
            var subscriptionDto = new SubscriptionDTO
            {
                CustomerId = dto.CustomerId,
                PlanId = dto.PlanId
                // The rest will be set by the service
            };
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
//{
//    private readonly ISubscriptionService _service;

//    public SubscriptionsController(ISubscriptionService service)
//    {
//        _service = service;
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create([FromBody] SubscriptionDTO dto)
//    {
//        var result = await _service.CreateSubscriptionAsync(dto);
//        return result ? Ok("Subscription created") : BadRequest("Already active or plan is full");
//    }

//    [HttpGet("/api/customers/{id}/subscriptions")]
//    public async Task<IActionResult> GetByCustomer(int id)
//    {
//        var result = await _service.GetSubscriptionsByCustomerAsync(id);
//        return Ok(result);
//    }
//}

