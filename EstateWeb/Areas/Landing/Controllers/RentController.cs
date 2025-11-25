using Microsoft.AspNetCore.Mvc;

namespace EstateWeb.Areas.Landing.Controllers
{
    [Area("Landing")]
    public class RentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
