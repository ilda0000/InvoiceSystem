using Microsoft.AspNetCore.Mvc;

namespace InvoiceSystem.Controllers
{
    public class DiscountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
