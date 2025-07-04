using InvoiceSystem.Models.Entity;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Models.Entity
{
    public class Payment: BaseEntity
    {
        
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public decimal AmountPaid { get; set; }

        // New relationship
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = null!;
    }

}
