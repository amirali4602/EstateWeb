using Microsoft.AspNetCore.Mvc;

namespace EstateWeb.Areas.Landing.Controllers
{
    [Area("Landing")]
    public class BuyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
