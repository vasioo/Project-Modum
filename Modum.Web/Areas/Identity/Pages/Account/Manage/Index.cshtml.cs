// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modum.Models.MainModel;
using System.ComponentModel.DataAnnotations;

namespace Modum.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

       
        [BindProperty]
        public InputModel Input { get; set; }

       
        public class InputModel
        {
            [Required(ErrorMessage = "The First Name field is required.")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "The Last Name field is required.")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "The Username field is required.")]
            public string Username { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            Username = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
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

        public async Task<IActionResult> OnPostAsync()
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

            if (string.IsNullOrWhiteSpace(Input.FirstName) || string.IsNullOrWhiteSpace(Input.LastName))
            {
                ModelState.AddModelError(string.Empty, "Both First Name and Last Name are required.");
                await LoadAsync(user);
                return Page();
            }

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.UserName = Input.Username;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                await LoadAsync(user);

                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Your profile has been updated";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await LoadAsync(user);
            }

            return Page();
        }


    }
}