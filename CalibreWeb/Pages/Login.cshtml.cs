/*
 * CalibreWeb
 * 
 * Copyright (C) 2018..2021 by Simon Baer
 *
 * This program is free software; you can redistribute it and/or modify it under the terms
 * of the GNU General Public License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along with this program;
 * If not, see http://www.gnu.org/licenses/.
 * 
 */

using System.Security.Claims;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

using CalibreWeb.Repository;
using CalibreWeb.Models;

namespace CalibreWeb.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        private readonly UserRepository userRepository;
        private readonly IStringLocalizer<SharedResource> loc;

        public LoginModel(UserRepository userRepository, IStringLocalizer<SharedResource> loc)
        {
            this.userRepository = userRepository;
            this.loc = loc;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (userRepository.CheckLogin(Username, Password, out var role))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, Username));

                if (role == AppUserRole.Admin)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, AppUserRole.Admin));
                    identity.AddClaim(new Claim(ClaimTypes.Role, AppUserRole.Download));
                } else if (role == AppUserRole.Download)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, AppUserRole.Download));
                }

                var principle = new ClaimsPrincipal(identity);
                await this.HttpContext.SignInAsync(principle).ConfigureAwait(false);

                return RedirectToPage("Index");
            }
            else
            {
                Message = loc["LoginError"];
                return Page();
            }
        }
    }
}
