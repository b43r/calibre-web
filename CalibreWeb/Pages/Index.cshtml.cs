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
using Microsoft.Extensions.Localization;

using CalibreWeb.Models;
using CalibreWeb.ViewModels;
using CalibreWeb.Repository;

namespace CalibreWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IStringLocalizer<SharedResource> loc;

        private readonly BookRepository bookRepository;
        private readonly AuthorRepository authorRepository;

        public IndexModel(CalibreContext db, IStringLocalizer<SharedResource> loc)
        {
            bookRepository = new BookRepository(db);
            authorRepository = new AuthorRepository(db);
            this.loc = loc;
        }

        public string Title { get; set; }

        public bool ShowAllLink { get; set; }

        public IEnumerable<BookVm> Books { get; set; }

        public void OnGet(long? authorId)
        {
            if (authorId.HasValue)
            {
                var author = authorRepository.GetAuthorById(authorId.Value);
                if (author != null)
                {
                    Books = bookRepository.GetBooksByAuthor(authorId.Value);

                    Title = loc["Books"] + $" - {author.Name} ({Books.Count()})";
                    ShowAllLink = true;
                    return;
                }
            }

            Books = bookRepository.GetAllBooks();
            Title = loc["Books"] + $" ({Books.Count()})";
            ShowAllLink = false;
        }
    }
}
