using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Service
{
    public interface IPlanService
    {
        Task<List<PlanDTO>> GetAllPlansAsync();
        Task<PlanDTO> GetByNameAsync(string name);
    }   
}