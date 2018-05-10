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

using CalibreWeb.Models;
using CalibreWeb.ViewModels;

namespace CalibreWeb.Repository
{
    public class BookRepository : Repository
    {
        public BookRepository(CalibreContext context)
            : base(context)
        {
        }

        public IEnumerable<BookVm> GetAllBooks()
        {
            return from b in CalibreContext.Books
                   join d in CalibreContext.Data on b.Id equals d.Book into dataGrp
                   join ba in CalibreContext.BooksAuthorsLink on b.Id equals ba.Book
                   join a in CalibreContext.Authors on ba.Author equals a.Id
                   join comment in CalibreContext.Comments on b.Id equals comment.Book into comments
                   from comment in comments.DefaultIfEmpty()
                   join bl in CalibreContext.BooksLanguagesLink on b.Id equals bl.Book
                   join l in CalibreContext.Languages on bl.LangCode equals l.Id                   
                   orderby b.Title
                   select new BookVm
                   {
                       Title = b.Title,
                       AuthorId = a.Id,
                       Author = a.Sort,
                       Comments = comment.Text,
                       Language = l.LangCode,
                       Path = b.Path,
                       HasCover = b.HasCover == "1",
                       Formats = dataGrp.OrderBy(d => d.Format).Select(x => new DataVm { Format = x.Format, FileName = x.Name + "." + x.Format.ToLower() }).ToList()
                   };
        }

        public IEnumerable<BookVm> GetBooksByAuthor(long authorId)
        {
            return from b in CalibreContext.Books
                   join d in CalibreContext.Data on b.Id equals d.Book into dataGrp
                   join ba in CalibreContext.BooksAuthorsLink on b.Id equals ba.Book
                   join a in CalibreContext.Authors on ba.Author equals a.Id
                   join comment in CalibreContext.Comments on b.Id equals comment.Book into comments
                   from comment in comments.DefaultIfEmpty()
                   join bl in CalibreContext.BooksLanguagesLink on b.Id equals bl.Book
                   join l in CalibreContext.Languages on bl.LangCode equals l.Id
                   where a.Id == authorId
                   orderby b.Title
                   select new BookVm
                   {
                       Title = b.Title,
                       AuthorId = a.Id,
                       Author = a.Sort,
                       Comments = comment.Text,
                       Language = l.LangCode,
                       Path = b.Path,
                       HasCover = b.HasCover == "1",
                       Formats = dataGrp.OrderBy(d => d.Format).Select(x => new DataVm { Format = x.Format, FileName = x.Name + "." + x.Format.ToLower() }).ToList()
                   };
        }

        private CalibreContext CalibreContext => base.dbContext as CalibreContext;
    }
}
