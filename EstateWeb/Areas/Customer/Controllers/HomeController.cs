using Estate.DataAccess.Data;
using Estate.Models;
using Estate.Utility;
using MD.PersianDateTime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Net;
using Elfie.Serialization;
using Polly;
using Newtonsoft.Json.Linq;
using System.Drawing;
using AspNetCore.SEOHelper.Sitemap;

namespace EstateWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
           

            IEnumerable<Page> pageList = _context.Pages.Where(x => x.isFeatured == true && x.isActive == true && x.Sold != true).ToList();
            
            ViewBag.featuredsDate = "";
            foreach (var objPage in pageList)
            {

                PersianDateTime persianDateTime = new PersianDateTime(objPage.Date);
                PersianDateTime persianDateTimeNow = new PersianDateTime(DateTime.Now);
                dynamic totaldays = (DateTime.Now - objPage.Date).Days;
                if(totaldays == 0)
                {
                    totaldays = "امروز";
                }
                else
                {
                    totaldays = totaldays + " روز پیش";
                }
                ViewBag.featuredsDate += totaldays + "*";
            }

            

            ViewBag.Employees = _context.ApplicationUsers.Where(x => x.Role != null).OrderBy(x=>x.order).ToList();
            string url = HttpContext.Request.GetDisplayUrl();
            ViewBag.Canonical = url;



            try
            {
                var list = new List<SitemapNode>();

                list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://hamid-estate.com/Customer/Home/Buy", Frequency = SitemapFrequency.Always });
                list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://hamid-estate.com/Customer/Home/Rent", Frequency = SitemapFrequency.Always });
                list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://hamid-estate.com/", Frequency = SitemapFrequency.Always });
                list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.4, Url = "https://hamid-estate.com/Customer/Home/AboutUs", Frequency = SitemapFrequency.Weekly });
                list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.4, Url = "https://hamid-estate.com/Customer/Home/ContactUs", Frequency = SitemapFrequency.Weekly });
                // Dynamic property pages
                IEnumerable<Page> propertyIds = _context.Pages.Where(x => x.isActive == true).ToList(); // Assume this method retrieves a list of property IDs from the database


                foreach (var pageId in propertyIds)

                {

                    list.Add(new SitemapNode

                    {

                        LastModified = DateTime.UtcNow,

                        Priority = 0.5, // Adjust priority as needed

                        Url = $"https://hamid-estate.com/Customer/Home/Property?pageId={pageId.PageId}",

                        Frequency = SitemapFrequency.Weekly // Adjust frequency as needed

                    });

                }
                foreach (var employee in ViewBag.Employees)

                {

                    if (employee.IsAgent == true) {
                        list.Add(new SitemapNode

                        {

                            LastModified = DateTime.UtcNow,

                            Priority = 0.5, // Adjust priority as needed

                            Url = $"https://hamid-estate.com/Customer/Agent?UserName={employee.UserName}",

                            Frequency = SitemapFrequency.Daily // Adjust frequency as needed

                        });
                    }

                }
                new SitemapDocument().CreateSitemapXML(list, _webHostEnvironment.WebRootPath);
            }
            catch (Exception e)

            {

                _logger.LogError(e, "An error occurred while generating the sitemap."); // Log the error


            }
            return View(pageList);
        }
        public IActionResult AboutUs()
        {
            string url = HttpContext.Request.GetDisplayUrl();
            ViewBag.Canonical = url;
            ViewBag.Employees = _context.ApplicationUsers.Where(x => x.Role != null).OrderBy(x => x.order).ToList();

            return View();

        }
        public IActionResult ContactUs()
        {
            string url = HttpContext.Request.GetDisplayUrl();
            ViewBag.Canonical = url;
            return View();

        }

        public ActionResult Buy(double? minPrice, double? maxPrice,int? rooms,int? minMeter, int? maxMeter, 
            string? query,bool? parking,bool? balcony,bool? elevator,bool? restored,
            int[]? CategoryId, int[]? CoolingId
            ,int[]? BuildingDirectionId, int[]? DocumentTypeId, int[]? HeatingId
            ,int[]? ToiletId, int[]? HotWaterSupplierId, int[]? FloorMaterialId)
        {
            IEnumerable<Page> pageList = _context.Pages.Where(x => x.isRent == false && x.isActive == true).ToList();
            if (rooms.HasValue) { 
                pageList = pageList.Where(x=>x.Rooms==rooms.Value).ToList();
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
            if (CategoryId?.Length > 0) {
                pageList = pageList.Where(x => CategoryId.Contains( x.CategoryId)).ToList();
            }

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
            pageList= pageList.OrderByDescending(x => x.Date);
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
                    image = (page.ImageUrl != null) ? "https://hamid-estate.com" + page.ImageUrl.Replace("\\","/") : "https://hamid-estate.com/images/logo.jpg",

                    url = "https://hamid-estate.com/Customer/Home/Property?pageId="+page.PageId


                })
            };
            ViewData["Schema"] = JsonConvert.SerializeObject(schema);

            string url = HttpContext.Request.GetDisplayUrl();
            ViewBag.Canonical = url;
            return View(pageList);
        }
        public ActionResult Rent(double? minPrice, double? maxPrice, int? rooms, int? minMeter, int? maxMeter,
            string? query, bool? parking, bool? balcony, bool? elevator, bool? restored,
            int[]? CategoryId, int[]? CoolingId
            , int[]? BuildingDirectionId, int[]? DocumentTypeId, int[]? HeatingId
            , int[]? ToiletId, int[]? HotWaterSupplierId, int[]? FloorMaterialId)
        {
            ViewBag.Featured = _context.Pages.Where(x => x.isFeatured == true && x.isRent == true && x.isActive == true).ToList();

            IEnumerable<Page> pageList = _context.Pages.Where(x => x.isRent == true && x.isActive == true).ToList();
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
                pageList = pageList.Where(x => x.Deposit >= minPrice.Value).ToList();
            }
            if (maxPrice.HasValue)
            {
                pageList = pageList.Where(x => x.Deposit <= maxPrice.Value).ToList();
            }
            if (CategoryId?.Length > 0)
            {
                pageList = pageList.Where(x => CategoryId.Contains(x.CategoryId)).ToList();
            }

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
        [Authorize(Roles = SD.Role_Customer +","+ SD.Role_Admin)]


        public IActionResult Ads()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["CoolingId"] = new SelectList(_context.Coolings, "Id", "Name");
            ViewData["BuildingDirectionId"] = new SelectList(_context.BuildingDirections, "Id", "Name");
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Name");
            ViewData["HeatingId"] = new SelectList(_context.Heatings, "Id", "Name");
            ViewData["ToiletId"] = new SelectList(_context.Toilet, "Id", "Name");
            ViewData["HotWaterSupplierId"] = new SelectList(_context.HotWaterSuppliers, "Id", "Name");
            ViewData["FloorMaterialId"] = new SelectList(_context.floorMaterials, "Id", "Name");
            string url = HttpContext.Request.GetDisplayUrl();
            ViewBag.Canonical = url;
            return View();
        }
        [Authorize(Roles = SD.Role_Customer + "," + SD.Role_Admin)]


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ads([Bind("PageId,CategoryId,ImageUrl,Title,Address,Description,PriceTotal,Meterage,Deposit,Rent,Year,Rooms,Floor,Units,TotalFloors,Elevator,Parking,StoreRoom,Balcony,Restored,DocumentTypeId,BuildingDirectionId,ToiletId,CoolingId,HeatingId,HotWaterSupplierId,FloorMaterialId,Gallery,isActive,isFeatured,PriceMeter,isRent,ShowCustomerNumber")] Page page, IFormFile? file, List<IFormFile>? files)
        {
            if (ModelState.IsValid)
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string pagePath = Path.Combine(wwwRootPath, @"Images\Pages");

                    using (var fileStream = new FileStream(Path.Combine(pagePath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    //ViewData["ImageUrl"]= @"\Images\Pages\"+ fileName;
                    page.ImageUrl = @"\Images\Pages\" + fileName;
                }
                if (files != null)
                {
                    if (files.Count != 0)
                    {
                        var AllGallery = "";
                        foreach (var f in files)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(f.FileName);
                            string pagePath = Path.Combine(wwwRootPath, @"Images\Pages");

                            using (var fileStream = new FileStream(Path.Combine(pagePath, fileName), FileMode.Create))
                            {
                                f.CopyTo(fileStream);
                            }

                            AllGallery += @"\Images\Pages\" + fileName + " ";
                        }
                        page.Gallery = AllGallery.Remove(AllGallery.Length - 1);
                    }
                }
                page.Date = DateTime.Now;
                var user = await _userManager.GetUserAsync(User);
                try{
                    page.CustomerNumber = user.UserName;
                    page.ShowCustomerNumber = true;
                }
                catch (Exception ex)
                { 

                }
                TempData["success"] = "آگهی ثبت گردید بعد از بازنگری در سایت نمایش داده میشود";
                _context.Add(page);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Ads));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", page.CategoryId);
            ViewData["CoolingId"] = new SelectList(_context.Coolings, "Id", "Name", page.CoolingId);
            ViewData["BuildingDirectionId"] = new SelectList(_context.BuildingDirections, "Id", "Name", page.BuildingDirectionId);
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Name", page.DocumentTypeId);
            ViewData["HeatingId"] = new SelectList(_context.Heatings, "Id", "Name", page.HeatingId);
            ViewData["ToiletId"] = new SelectList(_context.Toilet, "Id", "Name", page.ToiletId);
            ViewData["HotWaterSupplierId"] = new SelectList(_context.HotWaterSuppliers, "Id", "Name", page.HotWaterSupplierId);
            ViewData["FloorMaterialId"] = new SelectList(_context.floorMaterials, "Id", "Name", page.FloorMaterialId);
            string url = HttpContext.Request.GetDisplayUrl();
            ViewBag.Canonical = url;
            return View(page);
        }

        public ActionResult Property(int pageId)
        {
            Page? page = _context.Pages.Find(pageId);
            Category? category = _context.Categories.Find(page.CategoryId);
            Cooling? cooling = _context.Coolings.Find(page.CoolingId);
            DocumentType? documentType = _context.DocumentTypes.Find(page.DocumentTypeId);
            BuildingDirection? buildingDirection = _context.BuildingDirections.Find(page.BuildingDirectionId);
            Heating? heating = _context.Heatings.Find(page.HeatingId);
            Toilet? toilet = _context.Toilet.Find(page.ToiletId);
            HotWaterSupplier? hotWaterSupplier = _context.HotWaterSuppliers.Find(page.HotWaterSupplierId);
            FloorMaterial? floorMaterial = _context.floorMaterials.Find(page.FloorMaterialId);
            ViewBag.Category = category;
            ViewBag.Cooling = cooling;
            ViewBag.DocumentType = documentType;
            ViewBag.BuildingDirection = buildingDirection;
            ViewBag.Heating = heating;
            ViewBag.Toilet = toilet;
            ViewBag.HotWaterSupplier = hotWaterSupplier;
            ViewBag.FloorMaterial = floorMaterial;

            PersianDateTime persianDateTime = new PersianDateTime(page.Date);
            ViewData["Date"]= persianDateTime.ToShortDateTimeString();
            ViewBag.Creator = _context.ApplicationUsers.Where(x => x.UserName == page.CustomerNumber).FirstOrDefault();
            ViewBag.CustomerNumber = page.CustomerNumber;

            var schema = new
            {
                context = "https://schema.org/",
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


                
            };
            ViewData["Schema"] = JsonConvert.SerializeObject(schema);

            //ViewData["Date"]= page.Date;
            string url = HttpContext.Request.GetDisplayUrl();
            ViewBag.Canonical = url;
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                return View(page);

            }
            if (!page.isActive)
            {
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }


            return View(page);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
