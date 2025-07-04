using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Service
{
    public interface ICustomerService
    {
        Task<CustomerDTO> AddCustomerAsync(CustomerDTO dto); // ← return DTO instead of void
        Task<CustomerDTO?> GetByIdAsync(int id);
        Task<IEnumerable<CustomerDTO>> GetAllAsync(); 
    }
}
