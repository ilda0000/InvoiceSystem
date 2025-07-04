using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Models.Entity
{
    public class Customer : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(150)]
        public string Email { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    
}
}
