using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Service
{
    public interface IPlanService
    {
        Task<List<PlanDTO>> GetAllPlansAsync(); // symbol of money 
    }
}