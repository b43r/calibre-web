/*
 * CalibreWeb
 * 
 * Copyright (C) 2018 by Simon Baer
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
using System.Linq;

using Microsoft.AspNetCore.Mvc.RazorPages;

using CalibreWeb.Models;
using CalibreWeb.ViewModels;

namespace CalibreWeb.Pages
{
    public class AuthorModel : PageModel
    {
        private readonly CalibreContext db;

        public AuthorModel(CalibreContext db)
        {
            this.db = db;
        }

        public IEnumerable<AuthorVm> Authors { get; set; }

        public void OnGet()
        {
            Authors = from bal in db.BooksAuthorsLink
                        join a in db.Authors on bal.Author equals a.Id
                        group a by new { a.Id, a.Sort } into grp
                        orderby grp.Key.Sort
                        select new AuthorVm { Id = grp.Key.Id, Name = grp.Key.Sort, BookCount = grp.Count() };
        }
    }
}
