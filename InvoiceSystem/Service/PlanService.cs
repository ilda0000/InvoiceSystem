using AutoMapper;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Repositories.IRepositories;
using Microsoft.Extensions.Logging;

namespace InvoiceSystem.Service
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PlanService> _logger;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PlanService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
      
        public async Task<List<PlanDTO>> GetAllPlansAsync()
        {
            _logger.LogInformation("Fetching all subscription plans...");
            var plans = await _unitOfWork.Plans.GetAllAsync();

            if (plans == null || !plans.Any())
            {
                _logger.LogWarning("No plans found in the database.");
            }

            return _mapper.Map<List<PlanDTO>>(plans);
        }

        public Task<PlanDTO> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
