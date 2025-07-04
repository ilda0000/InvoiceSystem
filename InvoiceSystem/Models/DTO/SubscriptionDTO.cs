namespace InvoiceSystem.Models.DTO
{
    public class SubscriptionDTO
    {
        public int SubscriptionId { get; set; }
        public int CustomerId { get; set; }
        public int PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
