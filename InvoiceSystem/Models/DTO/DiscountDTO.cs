namespace InvoiceSystem.Models.DTO
{
    public class DiscountDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Fixed or Percentage
       // public decimal Value { get; set; }
        public int MinMonthsRequired { get; set; }
    }
}
