using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Service
{
    public interface IPlanService
    {
        Task CreatePlanAsync(PlanDTO planDto);
        Task<List<PlanDTO>> GetAllPlansAsync();
        Task GetByIdAsync(int id);

        //Task GetByIdAsync(int id);
        Task<PlanDTO> GetByNameAsync(string name);
    }   
}