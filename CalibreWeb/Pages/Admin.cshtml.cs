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

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using CalibreWeb.Repository;
using CalibreWeb.Models;
using CalibreWeb.Resources;

namespace CalibreWeb.Pages
{
    public class AdminModel : PageModel
    {
        private readonly UserRepository userRepository;
        private readonly LocService sharedLocalizer;

        public IEnumerable<AppUser> Users { get; set; }
        public List<SelectListItem> Roles { get; set; }

		public AdminModel(UserRepository userRepository, LocService sharedLocalizer)
        {
			this.userRepository = userRepository;
            this.sharedLocalizer = sharedLocalizer;
		}

        public void OnGet()
        {
			Users = userRepository.GetUsers();
            Roles = new List<SelectListItem>
            {
                new SelectListItem(sharedLocalizer.GetLocalizedHtmlString("RoleNone"), AppUserRole.None),
                new SelectListItem(sharedLocalizer.GetLocalizedHtmlString("RoleDownload"), AppUserRole.Download),
                new SelectListItem(sharedLocalizer.GetLocalizedHtmlString("RoleAdmin"), AppUserRole.Admin),
            };
        }
                
        public IActionResult OnPostCreate(string userName, string password, string passwordConfirm, string role)
        {
            if (userRepository.GetUser(userName) != null)
            {
                return new JsonResult(new { Success = false, Error = sharedLocalizer.GetLocalizedHtmlString("ErrorNameAlreadyExists").Value });
            }

            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                return new JsonResult(new { Success = false, Error = sharedLocalizer.GetLocalizedHtmlString("ErrorPasswordLength").Value });
            }

            if (password != passwordConfirm)
            {
                return new JsonResult(new { Success = false, Error = sharedLocalizer.GetLocalizedHtmlString("ErrorPasswordConfirm").Value });
            }

            userRepository.AddUser(userName, password, role);
            return new JsonResult(new { Success = true });
        }

        public IActionResult OnPostDelete(string userName)
        {
            if (userName.Equals(User.Identity.Name, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }

            userRepository.DeleteUser(userName);
            return new JsonResult(new { Success = true });
        }

        public IActionResult OnPostChangeRole(string userName, string role)
        {
            bool success = userRepository.UpdateRole(userName, role);
            return new JsonResult(new { Success = success });
        }
    }
}
