// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Api.Core;
using DataAccess.Persistence.Contexts;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Api.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly GoogleCaptchaService _captchaService;

        public RegisterConfirmationModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
             ApplicationDbContext context
            ,GoogleCaptchaService captchaService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _captchaService = captchaService;
        }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool DisplayConfirmAccountLink { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string PhoneNumberConfirmation { get; set; }
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>

            [Required]

            [Display(Name = "ConfirmationCode")]
            public string Code { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Role { get; set; }
            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }

            [Required]
            public string Token { get; set; }


        }
        [ViewData]
        public string PhoneNumber { get; set; }
        [ViewData]
        public string Code { get; set; }
        public async Task OnGetAsync(string Phone)
        {
            ViewData["Phone"] = Phone;
            //PhoneNumber = TempData["PhoneNumber"].ToString();
            //Code = TempData["Code"].ToString();
            if (!_roleManager.RoleExistsAsync(UserRoles.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(UserRoles.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(UserRoles.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(UserRoles.Role_Company)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(UserRoles.Role_Employee)).GetAwaiter().GetResult();
            }
            Input = new()
            {
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var confirmed = false;
            if (_context.smsDtos.Where(x => x.PhoneNumber == TempData["PhoneNumber"].ToString()) != null)
            {
                SmsDto mySms = _context.smsDtos.Where(x => x.PhoneNumber == TempData["PhoneNumber"].ToString()).FirstOrDefault();
                if (mySms.sentStatus == Input.Code)
                {
                    confirmed = true;
                }
                else
                {
                    mySms.FailedTimes = mySms.FailedTimes + 1;
                    _context.Update(mySms);
                    await _context.SaveChangesAsync();
                }

            }
            //Verify Response Token with google
            var captchaResult = await _captchaService.VerifyToken(Input.Token);
            if (!captchaResult)
            {
                return Page();
            }
            if (ModelState.IsValid && confirmed && TempData["PhoneNumber"]!=null)
            {
                var user = CreateUser();
                user.UserName = TempData["PhoneNumber"].ToString();
                user.PhoneNumber = TempData["PhoneNumber"].ToString();
                var result = await _userManager.CreateAsync(user, Input.Password);

                var user1 = await _userManager.FindByNameAsync(user.UserName);

                var codex = await _userManager.GeneratePasswordResetTokenAsync(user1);
                codex = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(codex));
                var CodexFinal = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(codex));
                var result2 = await _userManager.ResetPasswordAsync(user1, CodexFinal, Input.Password);

                returnUrl = "~/";
                if (result.Succeeded)
                {
                    TempData["success"] = "وارد شدید";
                    _logger.LogInformation("User created a new account with password.");

                    if (!string.IsNullOrEmpty(Input.Role))
                    {
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, UserRoles.Role_Customer);

                    }

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new {returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                if (result2.Succeeded)
                {
                    TempData["success"] = "وارد شدید";

                    _logger.LogInformation("User password updated.");

                    if (!string.IsNullOrEmpty(Input.Role))
                    {
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, UserRoles.Role_Customer);

                    }

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new {returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user1, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                foreach (var error in result2.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            TempData["error"] = "ورود ناموفق";
            // If we got this far, something failed, redisplay form
            return RedirectToPage("RegisterConfirmation", new { Phone = TempData["PhoneNumber"].ToString() });
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
