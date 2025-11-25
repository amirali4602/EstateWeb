using Estate.DataAccess.Data;
using Estate.Models;
using MD.PersianDateTime;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EstateWeb.Areas.Landing.Controllers
{
    [Area("Landing")]
    public class EliteController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly UserManager<IdentityUser> _userManager;
        public EliteController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public ActionResult Index(double? minPrice, double? maxPrice, int? rooms, int? minMeter, int? maxMeter,
            string? query, bool? parking, bool? balcony, bool? elevator, bool? restored,
            int[]? CoolingId
            , int[]? BuildingDirectionId, int[]? DocumentTypeId, int[]? HeatingId
            , int[]? ToiletId, int[]? HotWaterSupplierId, int[]? FloorMaterialId)
        {
            IEnumerable<Page> pageList = _context.Pages.Where(x => x.isRent == false && x.isActive == true).ToList();
            if (rooms.HasValue)
            {
                pageList = pageList.Where(x => x.Rooms == rooms.Value).ToList();
            }
            if (minMeter.HasValue)
            {
                pageList = pageList.Where(x => x.Meterage >= minMeter.Value).ToList();
            }
            if (maxMeter.HasValue)
            {
                pageList = pageList.Where(x => x.Meterage <= maxMeter.Value).ToList();
            }
            if (parking.HasValue)
            {
                pageList = pageList.Where(x => x.Parking == parking.Value).ToList();
            }
            if (balcony.HasValue)
            {
                pageList = pageList.Where(x => x.Balcony == balcony.Value).ToList();
            }
            if (elevator.HasValue)
            {
                pageList = pageList.Where(x => x.Elevator == elevator.Value).ToList();
            }
            if (restored.HasValue)
            {
                pageList = pageList.Where(x => x.Restored == restored.Value).ToList();
            }
            if (minPrice.HasValue)
            {
                pageList = pageList.Where(x => x.PriceTotal >= minPrice.Value).ToList();
            }
            if (maxPrice.HasValue)
            {
                pageList = pageList.Where(x => x.PriceTotal <= maxPrice.Value).ToList();
            }
            pageList = pageList.Where(x => x.CategoryId == 4).ToList();
            

            if (CoolingId?.Length > 0)
            {
                pageList = pageList.Where(x => CoolingId.Contains(x.CoolingId)).ToList();
            }
            if (BuildingDirectionId?.Length > 0)
            {
                pageList = pageList.Where(x => BuildingDirectionId.Contains(x.BuildingDirectionId)).ToList();
            }
            if (DocumentTypeId?.Length > 0)
            {
                pageList = pageList.Where(x => DocumentTypeId.Contains(x.DocumentTypeId)).ToList();
            }
            if (HeatingId?.Length > 0)
            {
                pageList = pageList.Where(x => HeatingId.Contains(x.HeatingId)).ToList();
            }
            if (ToiletId?.Length > 0)
            {
                pageList = pageList.Where(x => ToiletId.Contains(x.ToiletId)).ToList();
            }
            if (HotWaterSupplierId?.Length > 0)
            {
                pageList = pageList.Where(x => HotWaterSupplierId.Contains(x.HotWaterSupplierId)).ToList();
            }
            if (FloorMaterialId?.Length > 0)
            {
                pageList = pageList.Where(x => FloorMaterialId.Contains(x.FloorMaterialId)).ToList();
            }
            if (!string.IsNullOrEmpty(query))
            {
                pageList = pageList.Where(x => x.Title.Contains(query) || x.Address.Contains(query) || x.Description.Contains(query)).ToList();
            }
            pageList = pageList.Where(x => x.Description.Contains("دهکده ساحلی الیت")).ToList();
            
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
            ViewData["CategoryId"] = _context.Categories;
            ViewData["CoolingId"] = _context.Coolings;
            ViewData["BuildingDirectionId"] = _context.BuildingDirections;
            ViewData["DocumentTypeId"] = _context.DocumentTypes;
            ViewData["HeatingId"] = _context.Heatings;
            ViewData["ToiletId"] = _context.Toilet;
            ViewData["HotWaterSupplierId"] = _context.HotWaterSuppliers;
            ViewData["FloorMaterialId"] = _context.floorMaterials;

            var schema = new
            {
                context = "https://schema.org/",
                type = "ItemList",
                itemListElement = pageList.Take(10).Select(page => new

                {

                    type = "House",

                    name = page.Title,
                    address = new
                    {
                        type = "PostalAddress",
                        streetAddress = page.Address,
                        addressCountry = "Iran"
                    },

                    description = page.Description,

                    numberOfRooms = page.Rooms,
                    image = (page.ImageUrl != null) ? "https://hamid-estate.com" + page.ImageUrl.Replace("\\", "/") : "https://hamid-estate.com/images/logo.jpg",

                    url = "https://hamid-estate.com/Customer/Home/Property?pageId=" + page.PageId


                })
            };
            ViewData["Schema"] = JsonConvert.SerializeObject(schema);

            string url = HttpContext.Request.GetDisplayUrl();
            ViewBag.Canonical = url;
            return View(pageList);
        }

    }
}
