using Estate.Models;
using Microsoft.AspNetCore.Mvc;

namespace EstateWeb.Areas.Shop.Controllers
{
    [Area("Shop")]

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
           
            return View();
        }
    }
}
