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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using CalibreWeb.Models;
using CalibreWeb.Repository;

namespace CalibreWeb.Controllers
{
    [Authorize(Roles = AppUserRole.Download)]
    public class DownloadController : Controller
    {
        private IConfiguration configuration;
        private readonly BookRepository bookRepository;

        public DownloadController(CalibreContext db, IConfiguration configuration)
        {
            bookRepository = new BookRepository(db);
            this.configuration = configuration;
        }

        public IActionResult Index(int id, int formatId)
        {
            var book = bookRepository.GetBookById(id);
            if (book != null)
            {
                var format = book.Formats.FirstOrDefault(f => f.Id == formatId);
                if (format != null)
                {
                    string file = Path.Combine(configuration["Calibre:CataloguePath"], book.Path, format.FileName);
                    if (System.IO.File.Exists(file))
                    {
                        var fs = System.IO.File.OpenRead(file);
                        return File(fs, GetMimeType(Path.GetExtension(file)), Path.GetFileName(file));
                    }
                }
            }
            
            return new NotFoundResult();
        }

        private string GetMimeType(string ext)
        {
            switch (ext.ToLower())
            {
                case ".mobi":
                    return "application/x-mobipocket-ebook";
                case ".epub":
                    return "application/epub+zip";
                case ".azw3":
                    return "application/x-mobi8-ebook";
            }
            return "binary/octet-stream";
        }
    }
}