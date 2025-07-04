namespace InvoiceSystem.Models.DTO
{
    public class PlanDTO
    {
        public int Id { get; set; } // Unique identifier for the plan   
        public string Name { get; set; } // Basic, Pro, Enterprise  
        public int PricePerMonth { get; set; }
        public int MaxUsers { get; set; } // Maximum number of users allowed for the plan   

    }
}
