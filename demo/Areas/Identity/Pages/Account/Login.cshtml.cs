// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using demo.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using demo.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace demo.Areas.Identity.Pages.Account
{
	
	public class LoginModel : PageModel
    {
        private readonly UserManager<demoUser> _userManager;
        private readonly demoContext _context;
        private readonly SignInManager<demoUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<demoUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        public LoginModel(SignInManager<demoUser> signInManager, ILogger<LoginModel> logger, demoContext context, UserManager<demoUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            this.userManager = userManager;
            this._context = context;
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
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

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
            [Required]
			//[EmailAddress]
			//public string Email { get; set; }
			public string Phone { get; set; }
			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Required]

			//public string Password { get; set; }
			public DateTime DateOfBirth { get; set; }
			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
        //[HttpPost]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync();
        //    return RedirectToAction("Login", "Account");
        //}

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //returnUrl ??= Url.Content("~/");
            var a = Input.DateOfBirth;
            var b = Input.Phone;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                //var result = await _signInManager.PasswordSignInAsync(Input.Phone, Input.DateOfBirth, Input.RememberMe, lockoutOnFailure: false);
                var result = await _context.demoUser
      .SingleOrDefaultAsync(i => i.Phone == Input.Phone && i.DateOfBirth.Date == Input.DateOfBirth);

                if (result != null)
                {
                    var user = await _context.demoUser.SingleOrDefaultAsync(u => u.Phone == Input.Phone && u.DateOfBirth == Input.DateOfBirth);

                    _logger.LogInformation("User logged in.");

                    if (user != null)
                    {
                        if (user.UserRole == "Admin")
                        {
                            // For admin users, set up claims and sign in
                            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Input.Phone),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("UserId", user.Id),
                    // Additional claims if needed
                };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                            return RedirectToAction("Dashboard", "User");
                        }
                        else
                        {
                            // For non-admin users, redirect to userlogin and set claims
                            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Input.Phone),
                    new Claim("UserId", user.Id),
                      new Claim("IsCoordinator", "true"),
                    // Additional claims if needed
                };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                            if (user.IsCoordinator == false)
                            {
                                return RedirectToAction("userlogin", "Index", new { userId = user.Id });
                            }
                            else
                            {
                                return RedirectToAction("Cordinators", "CordinatorDashboard", new { userId = user.Id });
                                //return RedirectToAction("Cordinators", "Dashboard", new { userId = user.Id });

                            }

                        }
                    }
                    else
                    {
                        // Handle scenario where user is not found
                        return RedirectToAction("userlogin", "Index");
                    }
                }
                else
                {
                    // Handle failed login attempt
                    // ...
                }


                // Rest of your code handling other scenarios like RequiresTwoFactor, lockout, etc.

                //if (result.RequiresTwoFactor)
                //    {
                //        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                //    }
                //    if (result.IsLockedOut)
                //    {
                //        _logger.LogWarning("User account locked out.");
                //        return RedirectToPage("./Lockout");
                //    }
                //    else
                //    {
                //        ModelState.AddModelError(string.Empty, "Incorrect username or password");
                //        return Page();
                //    }
                //}
                if (result != null) { 
                if (await _userManager.GetTwoFactorEnabledAsync(result))
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }

                // Check if the account is locked out
                if (await _userManager.IsLockedOutAsync(result))
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
            
        }
            }
                // If we got this far, something failed, redisplay form
                ModelState.AddModelError(string.Empty, "Incorrect Phone number or date of birth");
            return Page();
        }
    }
}
