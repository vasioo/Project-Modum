﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modum.Models.MainModel;
using Modum.Models.BaseModels.Enums;
using NuGet.Versioning;
using System.ComponentModel.DataAnnotations;

namespace Modum.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public RegisterModel(
           UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger, IWebHostEnvironment webHostEnvironment,
            IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }


        [BindProperty]
        public InputModel Input { get; set; }


        public string ReturnUrl { get; set; }


        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public class InputModel
        {


            [Required(AllowEmptyStrings = false, ErrorMessage = "Email can not be empty!")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }


            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }


            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Membership")]
            public MembershipType MembershipType { get; set; } = MembershipType.NonPaymentActiveMember;

            [Display(Name = "Gender")]
            public GenderType Gender { get; set; }

            public DateTime AccountOriginDate { get; set; } = DateTime.Now;

            public long NumberOfCardTransactions { get; set; } = 0;
            public double TotalMoneySpent { get; set; } = 0;
            public DateTime LastOrderDate { get; set; } = DateTime.MinValue;
            public long MostFollowedCategoryId { get; set; } = 0;

            [Required(AllowEmptyStrings = false, ErrorMessage = "Country field can not be empty!")]
            [Display(Name = "Country")]
            public string Country { get; set; }

            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Date of Birth")]
            public DateTime BirthDate { get; set; }

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            if (await _userManager.GetUserAsync(HttpContext.User) != null)
            {
                Response.Redirect("/");
                return;
            }
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            try
            {

                var user = new ApplicationUser();

                user.UserName = Input.Email;
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.BirthDate = Input.BirthDate;

                user.Email = Input.Email;
                user.MembershipType = Input.MembershipType;
                user.Gender = Input.Gender;
                user.Country = Input.Country;

                var result = await _userManager.CreateAsync(user, Input.Password);


                var username = _configuration.GetSection("UserEmailSecrets:Username").Get<string>() ?? "";

                var checker = await _userManager.FindByEmailAsync(username);
                if (checker==null)
                {
                    var password = _configuration.GetSection("UserEmailSecrets:UserPassword").Get<string>() ?? "";
                    var firstName = _configuration.GetSection("UserEmailSecrets:FirstName").Get<string>() ?? "";
                    var lastName = _configuration.GetSection("UserEmailSecrets:LastName").Get<string>() ?? "";
                    var superAdminUser = new ApplicationUser();

                    superAdminUser.UserName = username;
                    superAdminUser.FirstName = firstName;
                    superAdminUser.LastName = lastName;
                    superAdminUser.BirthDate = DateTime.Now;

                    superAdminUser.Email = username;

                    var result2 = await _userManager.CreateAsync(superAdminUser, password);

                    if (result2.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                    }
                }

                if (result.Succeeded)
                {


                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);

                    #region Send Email

                    // _logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    //}
                    //else
                    //{
                    //  await _signInManager.SignInAsync(user, isPersistent: false);
                    //  return LocalRedirect(returnUrl);
                    //}
                    #endregion
                }

               
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return Page();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}