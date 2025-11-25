using Estate.Models;
using Microsoft.AspNetCore.Mvc;

namespace EstateWeb.Areas.Landing.Controllers
{
    [Area("Landing")]

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
           
            return View();
        }
    }
}
