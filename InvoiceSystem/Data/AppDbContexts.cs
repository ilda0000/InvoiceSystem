using Microsoft.EntityFrameworkCore;
using InvoiceSystem.Models.Entity;

namespace InvoiceSystem.Data
{
    public class AppDbContexts : DbContext
    {
        public AppDbContexts(DbContextOptions<AppDbContexts> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .Property(c => c.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Plan>()
                .Property(p => p.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<Subscription>()
                .Property(s => s.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<Payment>()
                .Property(p => p.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<Discount>()
                .Property(d => d.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<PaymentMethod>()
                .Property(pm => pm.Id)
                .UseIdentityColumn();

            base.OnModelCreating(modelBuilder);
        }
    }
}
