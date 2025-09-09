using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Models.Entity
{
    public class Discount : BaseEntity
    {
        // Required input from user or admin — editable, so needs both get and set
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        // User/admin must specify this — "Fixed" or "Percentage" — editable
        [Required(ErrorMessage = "Type is required (Fixed or Percentage)")]
        public string Type { get; set; } = string.Empty;

        // Should be assignable and editable — both get and set needed
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative.")]
        public decimal Value { get; set; }

        // Should be editable based on discount eligibility logic
        [Range(0, int.MaxValue, ErrorMessage = "MinMonthsRequired must be non-negative.")]
            public int MinMonthsRequired { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }
    }
}
