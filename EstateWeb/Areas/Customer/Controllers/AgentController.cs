using Estate.DataAccess.Data;
using MD.PersianDateTime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace EstateWeb.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class AgentController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        public AgentController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index(string? UserName)
        {
            if (UserName == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            var Employees = _context.ApplicationUsers.Where(x => x.Role != null && x.UserName == UserName && x.IsAgent).FirstOrDefault();
            if (Employees == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }

            var pageList = _context.Pages.Where(x => x.CustomerNumber == Employees.UserName && x.isActive);
            pageList = pageList.OrderByDescending(x => x.Date);
            ViewBag.pageDate = "";
            foreach (var objPage in pageList)
            {

                PersianDateTime persianDateTime = new PersianDateTime(objPage.Date);
                PersianDateTime persianDateTimeNow = new PersianDateTime(DateTime.Now);
                dynamic totaldays = (DateTime.Now - objPage.Date).Days;
                if (totaldays == 0)
                {
                    totaldays = "امروز";
                }
                else
                {
                    totaldays = totaldays + " روز پیش";
                }
                ViewBag.pageDate += totaldays + "*";
            }
            ViewBag.Employee = Employees;
            return View(pageList);
        }
    }
}
