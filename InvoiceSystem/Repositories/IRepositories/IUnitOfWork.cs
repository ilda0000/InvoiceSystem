using InvoiceSystem.Repositories.IRepositories;
        
namespace InvoiceSystem.Repositories.IRepositories  
{
    public interface IUnitOfWork
    {
        ICustomerRepository Customers { get; }
        IPlanRepository Plans { get; }
        ISubscriptionRepository Subscriptions { get; }
        IInvoiceRepository Invoices { get; }
        IPaymentRepository Payments { get; }
        IDiscountRepository Discounts { get; }
        IPaymentMethodRepository PaymentMethods { get; }
        Task SaveAsync();
    }
}