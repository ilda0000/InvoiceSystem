using InvoiceSystem.Models.Entity;

namespace InvoiceSystem.Repositories
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        Task<Customer?> GetByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync();
        // ICustomerRepository.cs
        Task<(bool NameTaken, bool EmailTaken)> WhichExistAsync(string name, string email);


    }
}