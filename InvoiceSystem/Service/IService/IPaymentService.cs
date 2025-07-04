using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Service
{
    public interface IPaymentService
    {
        Task<PaymentDTO?> AutoRegisterPaymentAsync();
    }
}