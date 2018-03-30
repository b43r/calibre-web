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

namespace CalibreWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CalibreContext db;
        private readonly IStringLocalizer<SharedResource> loc;

        public IndexModel(CalibreContext db, IStringLocalizer<SharedResource> loc)
        {
            this.db = db;
            this.loc = loc;
        }

        public string Title { get; set; }

        public bool ShowAllLink { get; set; }

        public IEnumerable<BookVm> Books { get; set; }

        public void OnGet(long? authorId)
        {
            if (authorId.HasValue)
            {
                var author = db.Authors.Find(authorId.Value);
                if (author != null)
                {
                    Books = from b in db.Books
                            join ba in db.BooksAuthorsLink on b.Id equals ba.Book
                            join a in db.Authors on ba.Author equals a.Id
                            join comment in db.Comments on b.Id equals comment.Book into comments
                            from comment in comments.DefaultIfEmpty()
                            join bl in db.BooksLanguagesLink on b.Id equals bl.Book
                            join l in db.Languages on bl.LangCode equals l.Id
                            where a.Id == authorId.Value
                            orderby b.Title
                            select new BookVm { Title = b.Title, AuthorId = a.Id, Author = a.Sort, Comments = comment.Text, Language = l.LangCode, Path = b.Path, HasCover = b.HasCover == "1", Formats = db.Data.Where(d => d.Book == b.Id).OrderBy(d => d.Format).ToList() };

                    Title = loc["Books"] + $" - {author.Sort} ({Books.Count()})";
                    ShowAllLink = true;
                    return;
                }
            }

            Books = from b in db.Books
                    join ba in db.BooksAuthorsLink on b.Id equals ba.Book
                    join a in db.Authors on ba.Author equals a.Id
                    join comment in db.Comments on b.Id equals comment.Book into comments
                    from comment in comments.DefaultIfEmpty()
                    join bl in db.BooksLanguagesLink on b.Id equals bl.Book
                    join l in db.Languages on bl.LangCode equals l.Id
                    orderby b.Title
                    select new BookVm { Title = b.Title, AuthorId = a.Id, Author = a.Sort, Comments = comment.Text, Language = l.LangCode, Path = b.Path, HasCover = b.HasCover == "1", Formats = db.Data.Where(d => d.Book == b.Id).OrderBy(d => d.Format).ToList() };
            Title = loc["Books"] + $" ({Books.Count()})";
            ShowAllLink = false;
        }
    }
}
