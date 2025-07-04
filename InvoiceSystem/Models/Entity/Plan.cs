using InvoiceSystem.Models.Entity;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Models.Entity   
{
    public class Plan : BaseEntity
    {
        public string Name { get; private set; }// Basic, Pro, Enterprise
        public decimal PricePerMonth { get; set; }
        public int MaxUsers { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }

    }
}
