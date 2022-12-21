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

namespace CalibreWeb.ViewModels
{
    public class BookVm
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public long AuthorId { get; set; }
        public string Comments { get; set; }
        public string Language { get; set; }
        public string Path { get; set; }
        public List<DataVm> Formats { get; set; }
        public bool HasCover { get; set; }
        public string SeriesName { get; set; }
        public double SeriesNumber { get; set; }

        public string TitleAndSeries
        {
            get
            {
                return Title + (!string.IsNullOrWhiteSpace(SeriesName) ? $" ({SeriesName + (SeriesNumber > 0 ? $" {SeriesNumber:##}" : string.Empty)})" : string.Empty);
            }
        }
    }
}
