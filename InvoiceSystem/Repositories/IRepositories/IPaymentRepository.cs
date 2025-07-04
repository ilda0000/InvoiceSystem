using InvoiceSystem.Models.Entity;

namespace InvoiceSystem.Repositories
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
    }
}