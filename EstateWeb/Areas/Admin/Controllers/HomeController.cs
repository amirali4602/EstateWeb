using Estate.DataAccess.Data;
using Estate.Models;
using Estate.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: BuildingDirections
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationUsers.ToListAsync());
        }


        // POST: BuildingDirections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name")] BuildingDirection buildingDirection)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(buildingDirection);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(buildingDirection);
        //}

        // GET: BuildingDirections/Edit/5
        public async Task<IActionResult> Edit(string? username)
        {
            if (username == null)
            {
                return NotFound();
            }

            var user = _context.ApplicationUsers.Where(x=> x.UserName == username).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
            }
            var user_us = _userManager.Users.Where(x => x.UserName == username).FirstOrDefault();
            var inrole = _userManager.GetRolesAsync(user_us).Result.FirstOrDefault();
            ViewData["RoleList"] = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
            {
                Text = i,
                Value = i,
                Selected = i == inrole
            });
            
            return View(user);
        }

        // POST: BuildingDirections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string username, [Bind("Name,Number,profilePic,Comment,Role,permisionDelete,permisionEdit,IsAgent,isRent,isOnlyRent,whatsApp,instagram,telegram,minRange,maxRange,UserName,FeaturedMax,order")] ApplicationUser user,string inRole, string? base64Image)
        {
            if (username != user.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var selectedUser = _context.ApplicationUsers.FirstOrDefault(x => x.UserName == username);
                    selectedUser.Name = user.Name;
                    selectedUser.Number = user.UserName;
                    selectedUser.Role = user.Role;
                    selectedUser.IsAgent = user.IsAgent;
                    selectedUser.isOnlyRent = user.isOnlyRent;
                    selectedUser.isRent = user.isRent;
                    selectedUser.permisionDelete = user.permisionDelete;
                    selectedUser.permisionEdit = user.permisionEdit;
                    selectedUser.Comment = user.Comment;
                    selectedUser.whatsApp = user.whatsApp;
                    selectedUser.telegram = user.telegram;
                    selectedUser.instagram = user.instagram;
                    selectedUser.FeaturedMax = user.FeaturedMax;
                    selectedUser.order = user.order;

                    

                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (base64Image != null)
                    {
                        byte[] imageBytes = Convert.FromBase64String(base64Image);

                        string fileName = Guid.NewGuid().ToString() + ".png";
                        string pagePath = Path.Combine(wwwRootPath, @"Images\Agents");
                        if (!string.IsNullOrEmpty(selectedUser.profilePic))
                        {
                            //delete the old image
                            var oldImagePath = Path.Combine(wwwRootPath, selectedUser.profilePic.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        await System.IO.File.WriteAllBytesAsync(Path.Combine(pagePath, fileName), imageBytes);

                        selectedUser.profilePic = @"\Images\Agents\" + fileName;
                    }
                    selectedUser.minRange = user.minRange;
                    selectedUser.maxRange = user.maxRange;
                    if (!String.IsNullOrEmpty(inRole))
                    {
                        var user_us = _userManager.Users.Where(x => x.UserName == username).FirstOrDefault();
                        var currentRole = _userManager.GetRolesAsync(user_us).Result.FirstOrDefault();
                        await _userManager.RemoveFromRoleAsync(user_us, currentRole);
                        await _userManager.AddToRoleAsync(user_us, inRole);

                    }
                    _context.Update(selectedUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetUsers()
        {
            
            List<ApplicationUser> objAppuserList = _context.ApplicationUsers.OrderByDescending(x=>x.order).ToList();
            return Json(new { data = objAppuserList });

            
        }
        #endregion

    }
}

