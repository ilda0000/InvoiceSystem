using System.Numerics;

namespace InvoiceSystem.Models.Entity
{
    public class Subscriptions : BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int PlanId { get; set; }
        public Plan Plan { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Invoice> Invoices { get; set; }

    }
}
