using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Models.Entity
{
    public class BaseEntity
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
