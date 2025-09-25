using InvoiceSystem.Models.Entity;

namespace InvoiceSystem.Models.Entity
{
    public class Invoice : BaseEntity
    {
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = null!;

        public int? DiscountId { get; set; }
        public Discount Discount { get; set; } = null!;
        public DateTime BillingDate { get; set; }
        public decimal TotalAmount { get; set; }
        public enum InvoiceStatus
        {
            NotPaid=0,Paid=1, 
        }
        public ICollection<Payment> Payments { get; set; }
        public InvoiceStatus Status { get; set; }
    }
}
