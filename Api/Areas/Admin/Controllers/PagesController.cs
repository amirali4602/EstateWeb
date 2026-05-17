using DataAccess.Persistence.Contexts;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Api.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = UserRoles.Role_Admin)]
public class PagesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public PagesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: Admin/Pages
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Pages.Include(p => p.Category);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Admin/Pages/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var page = await _context.Pages
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.PageId == id);
        if (page == null)
        {
            return NotFound();
        }

        return View(page);
    }

  
    // GET: Admin/Pages/Create
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        return View();
    }

    // POST: Admin/Pages/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PageId,CategoryId,ImageUrl,Title,Description,Price,Dimensions,Thickness,Mass,ColorCode,Gallery,isActive,isFeatured,PriceMeter,isRent")] Page page, string? base64Image, string? base64Images)
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
            _context.Add(page);
            await _context.SaveChangesAsync();
            TempData["success"] = "محصول ایجاد شد.";

            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", page.CategoryId);
        return View(page);
    }


    // GET: Admin/Pages/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var page = await _context.Pages.FindAsync(id);
        if (page == null)
        {
            return NotFound();
        }
        
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", page.CategoryId);
        return View(page);
    }

    // POST: Admin/Pages/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit([Bind("PageId,CategoryId,ImageUrl,Title,Description,Price,Dimensions,Thickness,Mass,ColorCode,Gallery,isActive,isFeatured,PriceMeter,isRent,CustomerNumber,AdsMessage,Sold")] Page page, string? base64Image, string? base64Images)
    {

        if (ModelState.IsValid)
        {

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (base64Image != null)
            {
                byte[] imageBytes = Convert.FromBase64String(base64Image);

                string fileName = Guid.NewGuid().ToString() + ".png";
                string pagePath = Path.Combine(wwwRootPath, @"Images\Pages");

                if (!string.IsNullOrEmpty(page.ImageUrl))
                {
                    //delete the old image
                    var oldImagePath = Path.Combine(wwwRootPath, page.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
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

                if (page.Gallery != null)
                {
                    var galleryDelete = page.Gallery.Split(" ");
                    foreach (var gd in galleryDelete)
                    {
                        //delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, gd.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                }
                page.Gallery = AllGallery.Remove(AllGallery.Length - 1);

            }
            try
            {
                page.Date = DateTime.Now;

                _context.Update(page);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageExists(page.PageId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["success"] = "تغییرات محصول اعمال شد";

            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", page.CategoryId);
        return View(page);
    }

    // GET: Admin/Pages/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var page = await _context.Pages
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.PageId == id);
        if (page == null)
        {
            return NotFound();
        }
        return View(page);
    }

    // POST: Admin/Pages/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Page id)
    {
        string wwwRootPath = _webHostEnvironment.WebRootPath;

        var page = await _context.Pages.FindAsync(id.PageId);
        if (page != null)
        {
            if (page.ImageUrl != null)
            {
                //delete the old image
                var oldImagePath = Path.Combine(wwwRootPath, page.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            if (page.Gallery != null)
            {
                var galleryDelete = page.Gallery.Split(" ");
                foreach (var gd in galleryDelete)
                {
                    //delete the old image
                    var oldImagesPath = Path.Combine(wwwRootPath, gd.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagesPath))
                    {
                        System.IO.File.Delete(oldImagesPath);
                    }
                }
            }
            _context.Pages.Remove(page);
        }

        await _context.SaveChangesAsync();
        TempData["success"] = "محصول پاک شد";
        return RedirectToAction(nameof(Index));
    }

    private bool PageExists(int id)
    {
        return _context.Pages.Any(e => e.PageId == id);
    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var agent = _context.ApplicationUsers;
        List<Page> objPageList = _context.Pages.Include(p => p.Category).ToList();
        dynamic myList = new List<dynamic>();
        dynamic User;
        dynamic Username;
        foreach (var objPage in objPageList)
        {
            if (objPage.CustomerNumber == null) {
                Username = "مشتری";
            }
            else
            {
                User = agent.Where(x => x.UserName == objPage.CustomerNumber).FirstOrDefault();
                Username = User.Name;
                if (User.Name == null)
                {
                    Username = "مشتری";
                }
            }
            myList.Add(new { objPage,agent =Username});
           
        }
        
        return Json(new { data = myList });

        
    }

    #endregion
}
