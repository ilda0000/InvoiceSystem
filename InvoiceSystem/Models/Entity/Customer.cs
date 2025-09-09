using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Models.Entity
{
    public class Customer : BaseEntity
    {
           
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
