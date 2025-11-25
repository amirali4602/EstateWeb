// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Azure;
using Estate.DataAccess.Data;
using Estate.Models;
using Estate.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace EstateWeb.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// 
        [Display(Name = "موبایل")]

        public string Username { get; set; }

        public IEnumerable<Estate.Models.Page> pageList { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "موبایل")]

            public string PhoneNumber { get; set; }

            public string Name { get; set; }
            public string Description { get; set; }
            public string Whatsapp { get; set; }
            public string Telegram { get; set; }
            public string Instagram { get; set; }
            public double minRange { get; set; }
            public double maxRange { get; set; }
            public string Role { get; set; }
            [Display(Name = "عکس پروفایل")]
            public string ProfilePic { get; set; }
            [Display(Name = "حداکثر نمایش ویژه مجاز")]
            public int FeaturedMax { get; set; }

        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            ApplicationUser appUser = _context.ApplicationUsers.Where(x => x.PhoneNumber == user.PhoneNumber).FirstOrDefault();
            var name = appUser?.Name;
            var description = appUser?.Comment;
            var whatsapp = appUser?.whatsApp;
            var telegram = appUser?.telegram;
            var instagram = appUser?.instagram;
            var minrange = appUser.minRange;
            var maxrange = appUser.maxRange;
            var role = appUser.Role;
            var profilepic = appUser.profilePic;
            var featuredmax = appUser.FeaturedMax;


            pageList = _context.Pages.Where(x=>x.CustomerNumber == userName).ToList();
            Username = userName;
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Name = name,
                Description = description,
                Whatsapp = whatsapp,
                Telegram = telegram,
                Instagram = instagram,
                minRange = minrange,
                maxRange = maxrange,
                Role = role,
                ProfilePic = profilepic,
                FeaturedMax = featuredmax,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? base64Image)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            Console.WriteLine(Input.Name);
            ApplicationUser appUser = _context.ApplicationUsers.Where(x => x.PhoneNumber == user.PhoneNumber).FirstOrDefault();


            appUser.Name = Input.Name;
            appUser.Comment = Input.Description;
            if (Input.Whatsapp.StartsWith("0"))
            {
                appUser.whatsApp = "98"+ Input.Whatsapp.Substring(1);
            }else if (Input.Whatsapp.StartsWith("98")){
                appUser.whatsApp = Input.Whatsapp;

            }
            if (Input.Telegram.Contains("https://t.me/"))
            {
                appUser.telegram = Input.Telegram.Replace("https://t.me/", "");

            }
            else
            {
                appUser.telegram = Input.Telegram;

            }
            appUser.instagram = Input.Instagram;

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (base64Image != null)
            {
                byte[] imageBytes = Convert.FromBase64String(base64Image);

                string fileName = Guid.NewGuid().ToString() + ".png";
                string pagePath = Path.Combine(wwwRootPath, @"Images\Agents");
                if (!string.IsNullOrEmpty(appUser.profilePic))
                {
                        //delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, appUser.profilePic.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                await System.IO.File.WriteAllBytesAsync(Path.Combine(pagePath, fileName), imageBytes);

                appUser.profilePic = @"\Images\Agents\" + fileName;
            }

            await _context.SaveChangesAsync();
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
