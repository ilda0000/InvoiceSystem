namespace InvoiceSystem.Models.DTO
{
    public class PlanDTO
    {
        public int Id { get; set; } // Unique identifier for the plan   
        public string Name { get; set; } = string.Empty; // Basic, Pro, Enterprise  
        public decimal PricePerMonth { get; set; }
        public int MaxUsers { get; set; } // Maximum number of users allowed for the plan   
    }
}
