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

namespace EstateWeb.Areas.Shop.Controllers;

[Area("Shop")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
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



        //try
        //{
        //    var list = new List<SitemapNode>();

        //    list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://hamid-estate.com/Customer/Home/Buy", Frequency = SitemapFrequency.Always });
        //    list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://hamid-estate.com/Customer/Home/Rent", Frequency = SitemapFrequency.Always });
        //    list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://hamid-estate.com/", Frequency = SitemapFrequency.Always });
        //    list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.4, Url = "https://hamid-estate.com/Customer/Home/AboutUs", Frequency = SitemapFrequency.Weekly });
        //    list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.4, Url = "https://hamid-estate.com/Customer/Home/ContactUs", Frequency = SitemapFrequency.Weekly });
        //    // Dynamic property pages
        //    IEnumerable<Page> propertyIds = _context.Pages.Where(x => x.isActive == true).ToList(); // Assume this method retrieves a list of property IDs from the database


        //    foreach (var pageId in propertyIds)

        //    {

        //        list.Add(new SitemapNode

        //        {

        //            LastModified = DateTime.UtcNow,

        //            Priority = 0.5, // Adjust priority as needed

        //            Url = $"https://hamid-estate.com/Customer/Home/Property?pageId={pageId.PageId}",

        //            Frequency = SitemapFrequency.Weekly // Adjust frequency as needed

        //        });

        //    }
        //    foreach (var employee in ViewBag.Employees)

        //    {

        //        if (employee.IsAgent == true) {
        //            list.Add(new SitemapNode

        //            {

        //                LastModified = DateTime.UtcNow,

        //                Priority = 0.5, // Adjust priority as needed

        //                Url = $"https://hamid-estate.com/Customer/Agent?UserName={employee.UserName}",

        //                Frequency = SitemapFrequency.Daily // Adjust frequency as needed

        //            });
        //        }

        //    }
        //    new SitemapDocument().CreateSitemapXML(list, _webHostEnvironment.WebRootPath);
        //}
        //catch (Exception e)

        //{

        //    _logger.LogError(e, "An error occurred while generating the sitemap."); // Log the error


        //}
       
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

    public ActionResult Buy(double? minPrice, double? maxPrice,
        string? query,int[]? CategoryId)
    {
        IEnumerable<Page> pageList = _context.Pages.Where(x =>x.isActive == true).ToList();
       
        if (CategoryId?.Length > 0) {
            pageList = pageList.Where(x => CategoryId.Contains( x.CategoryId)).ToList();
        }

        
        if (!string.IsNullOrEmpty(query))
        {
            pageList = pageList.Where(x => x.Title.Contains(query) || x.Description.Contains(query)).ToList();
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
                    addressCountry = "Iran"
                },

                description = page.Description,

                image = page.ImageUrl != null ? "https://hamid-estate.com" + page.ImageUrl.Replace("\\","/") : "https://hamid-estate.com/images/logo.jpg",

                url = "https://hamid-estate.com/Customer/Home/Property?pageId="+page.PageId


            })
        };
        ViewData["Schema"] = JsonConvert.SerializeObject(schema);

        string url = HttpContext.Request.GetDisplayUrl();
        ViewBag.Canonical = url;
        return View(pageList);
    }


   
    public ActionResult Property(int pageId)
    {
        Page? page = _context.Pages.Find(pageId);
        Category? category = _context.Categories.Find(page.CategoryId);
        ViewBag.Category = category;

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
                addressCountry = "Iran"
            },

            description = page.Description,

            image = page.ImageUrl != null ? "https://hamid-estate.com" + page.ImageUrl.Replace("\\", "/") : "https://hamid-estate.com/images/logo.jpg",

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
