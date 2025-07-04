using Swashbuckle.AspNetCore.Annotations;

namespace InvoiceSystem.Models.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
