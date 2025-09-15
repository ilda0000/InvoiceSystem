using AutoMapper;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Entity;
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

        public async Task<PlanDTO> GetPlanByIdAsync(int id)
        {
            _logger.LogInformation("Fetching plan with ID {Id}", id);
            var plan = await _unitOfWork.Plans.GetByIdAsync(id);

            if (plan == null)
            {
                _logger.LogWarning("Plan with ID {Id} not found", id);
                return null;
            }

            return _mapper.Map<PlanDTO>(plan);
        }
        public async Task GetByIdAsync(int id)
        {
            await GetPlanByIdAsync(id);
        }

        public async Task<PlanDTO> GetByNameAsync(string name)
        {
            _logger.LogInformation("Fetching plan with name {Name}", name);
            var plans = await _unitOfWork.Plans.GetAllAsync();
            var plan = plans.FirstOrDefault(p => p.Name == name);

            if (plan == null)
            {
                _logger.LogWarning("Plan with name {Name} not found", name);
                return null;
            }

            return _mapper.Map<PlanDTO>(plan);
        }

        public async Task CreatePlanAsync(PlanDTO planDto)
        {
            _logger.LogInformation("Creating a new plan...");
            var planEntity = _mapper.Map<Plan>(planDto);
            await _unitOfWork.Plans.AddAsync(planEntity);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Plan created successfully.");
        }
    }
}
