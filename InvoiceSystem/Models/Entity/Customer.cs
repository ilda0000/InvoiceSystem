using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Models.Entity
{
    public class Customer : BaseEntity
    {
       
        public string? Name { get; set; } 
        public string Email { get; set; } 
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    
}
}
