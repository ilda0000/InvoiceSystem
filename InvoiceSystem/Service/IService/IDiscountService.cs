using System.Threading.Tasks;
using InvoiceSystem.Models.DTO;

public interface IDiscountService
{
    Task CreateAsync(DiscountDTO dto);
}