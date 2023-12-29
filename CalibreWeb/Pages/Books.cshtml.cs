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
using Microsoft.Extensions.Localization;

using CalibreWeb.Models;
using CalibreWeb.ViewModels;
using CalibreWeb.Repository;

namespace CalibreWeb.Pages
{
    public class BooksModel : PageModel
    {
        private readonly IStringLocalizer<SharedResource> loc;

        private readonly BookRepository bookRepository;
        private readonly AuthorRepository authorRepository;

        public BooksModel(CalibreContext db, IStringLocalizer<SharedResource> loc)
        {
            bookRepository = new BookRepository(db);
            authorRepository = new AuthorRepository(db);
            this.loc = loc;
        }

        public string Title { get; set; }

        public bool ShowAllAuthors { get; set; }

        public bool ShowAllBooks { get; set; }

        public IEnumerable<BookVm> Books { get; set; }

        public void OnGet(long? bookId, long? authorId)
        {
            if (bookId.HasValue)
            {
                // show a single book
                Books = new[] { bookRepository.GetBookById(bookId.Value) };

                Title = loc["Books"];
                ShowAllBooks = true;
                return;
            }

            if (authorId.HasValue)
            {
                // show all books of the given author
                var author = authorRepository.GetAuthorById(authorId.Value);
                if (author != null)
                {
                    Books = bookRepository.GetBooksByAuthor(authorId.Value);

                    Title = loc["Books"] + $" - {author.Name}";
                    ShowAllAuthors = true;
					ViewData["ShowSearch"] = true;
					return;
                }
            }

            // show all books
            Books = bookRepository.GetAllBooks();
            Title = loc["Books"];
			ViewData["ShowSearch"] = true;
		}
    }
}
