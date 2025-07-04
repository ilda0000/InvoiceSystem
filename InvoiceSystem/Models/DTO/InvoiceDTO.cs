namespace InvoiceSystem.Models.DTO
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int SubscriptionId { get; set; }
        public DateTime BillingDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountApplied { get; set; }
        public bool Paid { get; set; }
        public string Status { get; set; }
        //public decimal DiscountApplied { get; set; }


    }
    public class InvoiceGenerationMinimalResponseDTO
    {
        public List<InvoiceDTO> Invoices { get; set; } = new();
    }
}
