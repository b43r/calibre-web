﻿/*
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

using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.RazorPages;

using CalibreWeb.Models;
using CalibreWeb.ViewModels;
using CalibreWeb.Repository;

namespace CalibreWeb.Pages
{
    public class AuthorModel : PageModel
    {
        private readonly AuthorRepository authorRepository;

        public AuthorModel(CalibreContext db)
        {
            authorRepository = new AuthorRepository(db);
        }

        public IEnumerable<AuthorVm> Authors { get; set; }

        public void OnGet()
        {
            Authors = authorRepository.GetAllAuthors();
            ViewData["ShowSearch"] = true;
		}
    }
}
