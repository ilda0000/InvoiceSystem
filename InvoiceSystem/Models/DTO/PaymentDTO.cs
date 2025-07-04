namespace InvoiceSystem.Models.DTO
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int InvoiceId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMethodName { get; set; } = string.Empty;
    }
}
