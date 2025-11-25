using Estate.DataAccess.Data;
using Estate.Models;
using Estate.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EstateWeb.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class AdsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly UserManager<IdentityUser> _userManager;

        public AdsController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Rent()
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
        public async Task<IActionResult> Rent([Bind("PageId,CategoryId,ImageUrl,Title,Address,Description,PriceTotal,Meterage,Deposit,Rent,Year,Rooms,Floor,Units,TotalFloors,Elevator,Parking,StoreRoom,Balcony,Restored,DocumentTypeId,BuildingDirectionId,ToiletId,CoolingId,HeatingId,HotWaterSupplierId,FloorMaterialId,Gallery,isActive,isFeatured,PriceMeter,isRent,ShowCustomerNumber")] Page page, string? base64Image, string? base64Images)
        {
            if (ModelState.IsValid)
            {
                page.isRent = true;
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (base64Image != null)
                {
                    byte[] imageBytes = Convert.FromBase64String(base64Image);

                    string fileName = Guid.NewGuid().ToString() + ".png";
                    string pagePath = Path.Combine(wwwRootPath, @"Images\Pages");


                    await System.IO.File.WriteAllBytesAsync(Path.Combine(pagePath, fileName), imageBytes);

                    page.ImageUrl = @"\Images\Pages\" + fileName;
                }
                if (base64Images != null)
                {
                    var allbytes = base64Images.Split("\n").SkipLast(1).ToArray();

                    var AllGallery = "";
                    foreach (var f in allbytes)
                    {
                        byte[] imageBytes = Convert.FromBase64String(f);


                        // Save the image or process it as needed

                        // Example: Saving to file system
                        string fileName = Guid.NewGuid().ToString() + ".png";
                        string pagePath = Path.Combine(wwwRootPath, @"Images\Pages");

                        //var filePath = Path.Combine("wwwroot/Images/Pages", "uploadedImage.png"); // Adjust path and filename as needed

                        await System.IO.File.WriteAllBytesAsync(Path.Combine(pagePath, fileName), imageBytes);

                        AllGallery += @"\Images\Pages\" + fileName + " ";
                    }
                    page.Gallery = AllGallery.Remove(AllGallery.Length - 1);

                }
                page.Date = DateTime.Now;
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    page.CustomerNumber = user.UserName;
                    page.ShowCustomerNumber = true;
                }
                catch (Exception ex)
                {

                }
                TempData["success"] = "آگهی ثبت گردید بعد از بازنگری در سایت نمایش داده میشود";
                _context.Add(page);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

        
        public IActionResult Buy()
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
        public async Task<IActionResult> Buy([Bind("PageId,CategoryId,ImageUrl,Title,Address,Description,PriceTotal,Meterage,Deposit,Rent,Year,Rooms,Floor,Units,TotalFloors,Elevator,Parking,StoreRoom,Balcony,Restored,DocumentTypeId,BuildingDirectionId,ToiletId,CoolingId,HeatingId,HotWaterSupplierId,FloorMaterialId,Gallery,isActive,isFeatured,PriceMeter,isRent,ShowCustomerNumber")] Page page, string? base64Image, string? base64Images)
        {
            if (ModelState.IsValid)
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (base64Image != null)
                {
                    byte[] imageBytes = Convert.FromBase64String(base64Image);

                    string fileName = Guid.NewGuid().ToString() + ".png";
                    string pagePath = Path.Combine(wwwRootPath, @"Images\Pages");


                    await System.IO.File.WriteAllBytesAsync(Path.Combine(pagePath, fileName), imageBytes);

                    page.ImageUrl = @"\Images\Pages\" + fileName;
                }
                if (base64Images != null)
                {
                    var allbytes = base64Images.Split("\n").SkipLast(1).ToArray();

                    var AllGallery = "";
                    foreach (var f in allbytes)
                    {
                        byte[] imageBytes = Convert.FromBase64String(f);


                        // Save the image or process it as needed

                        // Example: Saving to file system
                        string fileName = Guid.NewGuid().ToString() + ".png";
                        string pagePath = Path.Combine(wwwRootPath, @"Images\Pages");

                        //var filePath = Path.Combine("wwwroot/Images/Pages", "uploadedImage.png"); // Adjust path and filename as needed

                        await System.IO.File.WriteAllBytesAsync(Path.Combine(pagePath, fileName), imageBytes);

                        AllGallery += @"\Images\Pages\" + fileName + " ";
                    }
                    page.Gallery = AllGallery.Remove(AllGallery.Length - 1);

                }
                page.Date = DateTime.Now;
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    page.CustomerNumber = user.UserName;
                    page.ShowCustomerNumber = true;
                }
                catch (Exception ex)
                {

                }
                TempData["success"] = "آگهی ثبت گردید بعد از بازنگری در سایت نمایش داده میشود";
                _context.Add(page);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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



    }
}
