using Microsoft.AspNetCore.Mvc;

namespace EstateWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PropertyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
