namespace InvoiceSystem.Models.Entity
{
    public class PaymentMethod : BaseEntity
    {
        public string Name { get; set; }           // e.g. "Credit Card", "Debit Card", "Bank Transfer"
        public string Description { get; set; }    // Optional: e.g. "Visa/MasterCard accepted"
        public string? Provider { get; set; }      // Optional: "Visa", "Mastercard", "Raiffeisen", etc.
        public bool IsActive { get; set; }         // In case a method is no longer used
        public ICollection<Payment>? Payments { get; set; }
    }

}
