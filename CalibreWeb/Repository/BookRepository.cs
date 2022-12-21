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
                   join ba in CalibreContext.BooksAuthorsLink on b.Id equals ba.Book
                   join a in CalibreContext.Authors on ba.Author equals a.Id
                   join comment in CalibreContext.Comments on b.Id equals comment.Book into comments
                   from comment in comments.DefaultIfEmpty()
                   join bl in CalibreContext.BooksLanguagesLink on b.Id equals bl.Book
                   join l in CalibreContext.Languages on bl.LangCode equals l.Id
                   join bs in CalibreContext.BooksSeriesLink on b.Id equals bs.Book into book_series_link
                   from bs in book_series_link.DefaultIfEmpty()
                   join s in CalibreContext.Series on bs.Series equals s.Id into series
                   from s in series.DefaultIfEmpty()
                   orderby b.Title
                   select new BookVm
                   {
                       Id = b.Id,
                       Title = b.Title,
                       AuthorId = a.Id,
                       Author = a.Sort,
                       Comments = comment.Text,
                       Language = l.LangCode,
                       Path = b.Path,
                       HasCover = b.HasCover == "1",
                       Formats = (from d in CalibreContext.Data where d.Book == b.Id select new DataVm { Id = d.Id, Format = d.Format, FileName = d.Name + "." + d.Format.ToLower() }).ToList(),
                       SeriesName = s.Name,
                       SeriesNumber = b.SeriesIndex
            };
        }

        public IEnumerable<BookVm> GetBooksByAuthor(long authorId)
        {
            return from b in CalibreContext.Books
                   join ba in CalibreContext.BooksAuthorsLink on b.Id equals ba.Book
                   join a in CalibreContext.Authors on ba.Author equals a.Id
                   join comment in CalibreContext.Comments on b.Id equals comment.Book into comments
                   from comment in comments.DefaultIfEmpty()
                   join bl in CalibreContext.BooksLanguagesLink on b.Id equals bl.Book
                   join l in CalibreContext.Languages on bl.LangCode equals l.Id
                   join bs in CalibreContext.BooksSeriesLink on b.Id equals bs.Book into book_series_link
                   from bs in book_series_link.DefaultIfEmpty()
                   join s in CalibreContext.Series on bs.Series equals s.Id into series
                   from s in series.DefaultIfEmpty()
                   where a.Id == authorId
                   orderby b.Title
                   select new BookVm
                   {
                       Id = b.Id,
                       Title = b.Title,
                       AuthorId = a.Id,
                       Author = a.Sort,
                       Comments = comment.Text,
                       Language = l.LangCode,
                       Path = b.Path,
                       HasCover = b.HasCover == "1",
                       Formats = (from d in CalibreContext.Data where d.Book == b.Id select new DataVm { Id = d.Id, Format = d.Format, FileName = d.Name + "." + d.Format.ToLower() }).ToList(),
                       SeriesName = s.Name,
                       SeriesNumber = b.SeriesIndex
                   };
        }

        public BookVm GetBookById(long bookId)
        {
            var query = from b in CalibreContext.Books
                   join ba in CalibreContext.BooksAuthorsLink on b.Id equals ba.Book
                   join a in CalibreContext.Authors on ba.Author equals a.Id
                   join comment in CalibreContext.Comments on b.Id equals comment.Book into comments
                   from comment in comments.DefaultIfEmpty()
                   join bl in CalibreContext.BooksLanguagesLink on b.Id equals bl.Book
                   join l in CalibreContext.Languages on bl.LangCode equals l.Id
                   join bs in CalibreContext.BooksSeriesLink on b.Id equals bs.Book into book_series_link
                   from bs in book_series_link.DefaultIfEmpty()
                   join s in CalibreContext.Series on bs.Series equals s.Id into series
                   from s in series.DefaultIfEmpty()
                   where b.Id == bookId
                   orderby b.Title
                   select new BookVm
                   {
                       Id = b.Id,
                       Title = b.Title,
                       AuthorId = a.Id,
                       Author = a.Sort,
                       Comments = comment.Text,
                       Language = l.LangCode,
                       Path = b.Path,
                       HasCover = b.HasCover == "1",
                       Formats = (from d in CalibreContext.Data where d.Book == b.Id select new DataVm { Id = d.Id, Format = d.Format, FileName = d.Name + "." + d.Format.ToLower() }).ToList(),
                       SeriesName = s.Name,
                       SeriesNumber = b.SeriesIndex
                   };
            return query.FirstOrDefault();
        }

        private CalibreContext CalibreContext => base.dbContext as CalibreContext;
    }
}
