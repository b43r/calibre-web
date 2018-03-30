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

namespace CalibreWeb.Models
{
    public partial class Books
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Sort { get; set; }
        public string Timestamp { get; set; }
        public string Pubdate { get; set; }
        public double SeriesIndex { get; set; }
        public string AuthorSort { get; set; }
        public string Isbn { get; set; }
        public string Lccn { get; set; }
        public string Path { get; set; }
        public long Flags { get; set; }
        public string Uuid { get; set; }
        public string HasCover { get; set; }
        public string LastModified { get; set; }
    }
}
