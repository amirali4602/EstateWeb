// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Estate.DataAccess.Data;
using Estate.Models;
using Estate.Utility;
using EstateWeb.Areas.Admin.Controllers;
using EstateWeb.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EstateWeb.Areas.Identity.Pages.Account
{

    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly GoogleCaptchaService _captchaService;
        public RegisterModel(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, ApplicationDbContext context
            , GoogleCaptchaService captchaService)
        {
            _roleManager= roleManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _captchaService = captchaService;
        }

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
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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

            [Required(ErrorMessage = "شماره موبایل اجباری است")]
            [Phone(ErrorMessage = "شماره وارد شده صحیح نیست")]
            [Display(Name = "شماره موبایل")]
            public string PhoneNumber { get; set; }

            [Required]
            public string Token { get; set; }

        }


        public async Task OnGetAsync(string? phoneNumber,string? title,string returnUrl = null)
        {
            ViewData["changetitle"] = title;
            ViewData["phoneNumber"] = phoneNumber;
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string? phoneNumber,string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //Verify Response Token with google
            var captchaResult = await _captchaService.VerifyToken(Input.Token);
            if (!captchaResult)
            {
                return Page();
            }
            if (ModelState.IsValid)
            {
                SmsDto smsDto = _context.smsDtos.Where(x => x.PhoneNumber == Input.PhoneNumber).FirstOrDefault();
                if (smsDto == null)
                {
                    SmsDto smsDto1 = new SmsDto();
                    smsDto1.PhoneNumber = Input.PhoneNumber;
                    smsDto1.date = DateTime.Now;
                    smsDto1.FailedTimes = 0;
                    smsDto1.sentStatus = "777";
                    SmsSender sms = new SmsSender();
                    var smsPass = sms.sendmessage(Input.PhoneNumber);
                    smsDto1.sentStatus = smsPass;

                    TempData["success"] = "کد به شماره " + Input.PhoneNumber.ToString() + " ارسال شد";

                    
                    _context.Add(smsDto1);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    smsDto.FailedTimes = smsDto.FailedTimes + 1;
                    smsDto.date = DateTime.Now;

                    smsDto.sentStatus = "777";
                    SmsSender sms = new SmsSender();
                    var smsPass = sms.sendmessage(Input.PhoneNumber);
                    smsDto.sentStatus = smsPass;
                    TempData["success"] = "کد به شماره " + Input.PhoneNumber.ToString() + " ارسال شد";

                    
                    _context.Update(smsDto);
                    await _context.SaveChangesAsync();
                }

                phoneNumber = Input.PhoneNumber;
                return RedirectToPage("RegisterConfirmation" , new { Phone=phoneNumber });
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
