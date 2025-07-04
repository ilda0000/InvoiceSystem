using InvoiceSystem.Data;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Repositories.IRepositories;
using System;

namespace InvoiceSystem.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContexts _context;

        public UnitOfWork(
            ICustomerRepository customers,
            IPlanRepository plans,
            ISubscriptionRepository subscriptions,
            IInvoiceRepository invoices,
            IPaymentRepository payments,
            IPaymentMethodRepository paymentMethods,
            IDiscountRepository discounts,
            AppDbContexts context)
        {
            Customers = customers;
            Plans = plans;
            Subscriptions = subscriptions;
            Invoices = invoices;
            Payments = payments;
            PaymentMethods = paymentMethods;
            Discounts = discounts;
            _context = context;
        }

        public ICustomerRepository Customers { get; }
        public IPlanRepository Plans { get; }
        public ISubscriptionRepository Subscriptions { get; }
        public IInvoiceRepository Invoices { get; }
        public IPaymentRepository Payments { get; }
        public IPaymentMethodRepository PaymentMethods { get; }
        public IDiscountRepository Discounts { get; }

        public AppDbContexts Context => _context;

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
