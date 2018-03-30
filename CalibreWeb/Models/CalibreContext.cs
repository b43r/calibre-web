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

using Microsoft.EntityFrameworkCore;

namespace CalibreWeb.Models
{
    public partial class CalibreContext : DbContext
    {
        public virtual DbSet<Authors> Authors { get; set; }
        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<BooksAuthorsLink> BooksAuthorsLink { get; set; }
        public virtual DbSet<BooksLanguagesLink> BooksLanguagesLink { get; set; }
        public virtual DbSet<BooksPluginData> BooksPluginData { get; set; }
        public virtual DbSet<BooksPublishersLink> BooksPublishersLink { get; set; }
        public virtual DbSet<BooksRatingsLink> BooksRatingsLink { get; set; }
        public virtual DbSet<BooksSeriesLink> BooksSeriesLink { get; set; }
        public virtual DbSet<BooksTagsLink> BooksTagsLink { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<ConversionOptions> ConversionOptions { get; set; }
        public virtual DbSet<CustomColumns> CustomColumns { get; set; }
        public virtual DbSet<Data> Data { get; set; }
        public virtual DbSet<Feeds> Feeds { get; set; }
        public virtual DbSet<Identifiers> Identifiers { get; set; }
        public virtual DbSet<Languages> Languages { get; set; }
        public virtual DbSet<LibraryId> LibraryId { get; set; }
        public virtual DbSet<MetadataDirtied> MetadataDirtied { get; set; }
        public virtual DbSet<Preferences> Preferences { get; set; }
        public virtual DbSet<Publishers> Publishers { get; set; }
        public virtual DbSet<Ratings> Ratings { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }

        public CalibreContext(DbContextOptions<CalibreContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authors>(entity =>
            {
                entity.ToTable("authors");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasColumnName("link")
                    .HasDefaultValueSql("\"\"");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Sort).HasColumnName("sort");
            });

            modelBuilder.Entity<Books>(entity =>
            {
                entity.ToTable("books");

                entity.HasIndex(e => e.AuthorSort)
                    .HasName("authors_idx");

                entity.HasIndex(e => e.Sort)
                    .HasName("books_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuthorSort).HasColumnName("author_sort");

                entity.Property(e => e.Flags)
                    .HasColumnName("flags")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.HasCover)
                    .HasColumnName("has_cover")
                    .HasColumnType("BOOL")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Isbn)
                    .HasColumnName("isbn")
                    .HasDefaultValueSql("\"\"");

                entity.Property(e => e.LastModified)
                    .IsRequired()
                    .HasColumnName("last_modified")
                    .HasColumnType("TIMESTAMP")
                    .HasDefaultValueSql("\"2000-01-01 00:00:00+00:00\"");

                entity.Property(e => e.Lccn)
                    .HasColumnName("lccn")
                    .HasDefaultValueSql("\"\"");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnName("path")
                    .HasDefaultValueSql("\"\"");

                entity.Property(e => e.Pubdate)
                    .HasColumnName("pubdate")
                    .HasColumnType("TIMESTAMP")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.SeriesIndex)
                    .HasColumnName("series_index")
                    .HasDefaultValueSql("1.0");

                entity.Property(e => e.Sort).HasColumnName("sort");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("TIMESTAMP")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasDefaultValueSql("'Unknown'");

                entity.Property(e => e.Uuid).HasColumnName("uuid");
            });

            modelBuilder.Entity<BooksAuthorsLink>(entity =>
            {
                entity.ToTable("books_authors_link");

                entity.HasIndex(e => e.Author)
                    .HasName("books_authors_link_aidx");

                entity.HasIndex(e => e.Book)
                    .HasName("books_authors_link_bidx");

                entity.HasIndex(e => new { e.Book, e.Author })
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Author).HasColumnName("author");

                entity.Property(e => e.Book).HasColumnName("book");
            });

            modelBuilder.Entity<BooksLanguagesLink>(entity =>
            {
                entity.ToTable("books_languages_link");

                entity.HasIndex(e => e.Book)
                    .HasName("books_languages_link_bidx");

                entity.HasIndex(e => e.LangCode)
                    .HasName("books_languages_link_aidx");

                entity.HasIndex(e => new { e.Book, e.LangCode })
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Book).HasColumnName("book");

                entity.Property(e => e.ItemOrder)
                    .HasColumnName("item_order")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LangCode).HasColumnName("lang_code");
            });

            modelBuilder.Entity<BooksPluginData>(entity =>
            {
                entity.ToTable("books_plugin_data");

                entity.HasIndex(e => new { e.Book, e.Name })
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Book).HasColumnName("book");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Val)
                    .IsRequired()
                    .HasColumnName("val");
            });

            modelBuilder.Entity<BooksPublishersLink>(entity =>
            {
                entity.ToTable("books_publishers_link");

                entity.HasIndex(e => e.Book)
                    .HasName("books_publishers_link_bidx");

                entity.HasIndex(e => e.Publisher)
                    .HasName("books_publishers_link_aidx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Book).HasColumnName("book");

                entity.Property(e => e.Publisher).HasColumnName("publisher");
            });

            modelBuilder.Entity<BooksRatingsLink>(entity =>
            {
                entity.ToTable("books_ratings_link");

                entity.HasIndex(e => e.Book)
                    .HasName("books_ratings_link_bidx");

                entity.HasIndex(e => e.Rating)
                    .HasName("books_ratings_link_aidx");

                entity.HasIndex(e => new { e.Book, e.Rating })
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Book).HasColumnName("book");

                entity.Property(e => e.Rating).HasColumnName("rating");
            });

            modelBuilder.Entity<BooksSeriesLink>(entity =>
            {
                entity.ToTable("books_series_link");

                entity.HasIndex(e => e.Book)
                    .HasName("books_series_link_bidx");

                entity.HasIndex(e => e.Series)
                    .HasName("books_series_link_aidx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Book).HasColumnName("book");

                entity.Property(e => e.Series).HasColumnName("series");
            });

            modelBuilder.Entity<BooksTagsLink>(entity =>
            {
                entity.ToTable("books_tags_link");

                entity.HasIndex(e => e.Book)
                    .HasName("books_tags_link_bidx");

                entity.HasIndex(e => e.Tag)
                    .HasName("books_tags_link_aidx");

                entity.HasIndex(e => new { e.Book, e.Tag })
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Book).HasColumnName("book");

                entity.Property(e => e.Tag).HasColumnName("tag");
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.ToTable("comments");

                entity.HasIndex(e => e.Book)
                    .HasName("comments_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Book).HasColumnName("book");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasColumnName("text");
            });

            modelBuilder.Entity<ConversionOptions>(entity =>
            {
                entity.ToTable("conversion_options");

                entity.HasIndex(e => e.Book)
                    .HasName("conversion_options_idx_b");

                entity.HasIndex(e => e.Format)
                    .HasName("conversion_options_idx_a");

                entity.HasIndex(e => new { e.Format, e.Book })
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Book).HasColumnName("book");

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnName("data");

                entity.Property(e => e.Format)
                    .IsRequired()
                    .HasColumnName("format");
            });

            modelBuilder.Entity<CustomColumns>(entity =>
            {
                entity.ToTable("custom_columns");

                entity.HasIndex(e => e.Label)
                    .HasName("custom_columns_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Datatype)
                    .IsRequired()
                    .HasColumnName("datatype");

                entity.Property(e => e.Display)
                    .IsRequired()
                    .HasColumnName("display")
                    .HasDefaultValueSql("\"{}\"");

                entity.Property(e => e.Editable)
                    .IsRequired()
                    .HasColumnName("editable")
                    .HasColumnType("BOOL")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.IsMultiple)
                    .IsRequired()
                    .HasColumnName("is_multiple")
                    .HasColumnType("BOOL")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasColumnName("label");

                entity.Property(e => e.MarkForDelete)
                    .IsRequired()
                    .HasColumnName("mark_for_delete")
                    .HasColumnType("BOOL")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Normalized)
                    .IsRequired()
                    .HasColumnName("normalized")
                    .HasColumnType("BOOL");
            });

            modelBuilder.Entity<Data>(entity =>
            {
                entity.ToTable("data");

                entity.HasIndex(e => e.Book)
                    .HasName("data_idx");

                entity.HasIndex(e => e.Format)
                    .HasName("formats_idx");

                entity.HasIndex(e => new { e.Book, e.Format })
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Book).HasColumnName("book");

                entity.Property(e => e.Format)
                    .IsRequired()
                    .HasColumnName("format");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.UncompressedSize).HasColumnName("uncompressed_size");
            });

            modelBuilder.Entity<Feeds>(entity =>
            {
                entity.ToTable("feeds");

                entity.HasIndex(e => e.Title)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Script)
                    .IsRequired()
                    .HasColumnName("script");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Identifiers>(entity =>
            {
                entity.ToTable("identifiers");

                entity.HasIndex(e => new { e.Book, e.Type })
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Book).HasColumnName("book");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasDefaultValueSql("\"isbn\"");

                entity.Property(e => e.Val)
                    .IsRequired()
                    .HasColumnName("val");
            });

            modelBuilder.Entity<Languages>(entity =>
            {
                entity.ToTable("languages");

                entity.HasIndex(e => e.LangCode)
                    .HasName("languages_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.LangCode)
                    .IsRequired()
                    .HasColumnName("lang_code");
            });

            modelBuilder.Entity<LibraryId>(entity =>
            {
                entity.ToTable("library_id");

                entity.HasIndex(e => e.Uuid)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Uuid)
                    .IsRequired()
                    .HasColumnName("uuid");
            });

            modelBuilder.Entity<MetadataDirtied>(entity =>
            {
                entity.ToTable("metadata_dirtied");

                entity.HasIndex(e => e.Book)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Book).HasColumnName("book");
            });

            modelBuilder.Entity<Preferences>(entity =>
            {
                entity.ToTable("preferences");

                entity.HasIndex(e => e.Key)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("key");

                entity.Property(e => e.Val)
                    .IsRequired()
                    .HasColumnName("val");
            });

            modelBuilder.Entity<Publishers>(entity =>
            {
                entity.ToTable("publishers");

                entity.HasIndex(e => e.Name)
                    .HasName("publishers_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Sort).HasColumnName("sort");
            });

            modelBuilder.Entity<Ratings>(entity =>
            {
                entity.ToTable("ratings");

                entity.HasIndex(e => e.Rating)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Rating).HasColumnName("rating");
            });

            modelBuilder.Entity<Series>(entity =>
            {
                entity.ToTable("series");

                entity.HasIndex(e => e.Name)
                    .HasName("series_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Sort).HasColumnName("sort");
            });

            modelBuilder.Entity<Tags>(entity =>
            {
                entity.ToTable("tags");

                entity.HasIndex(e => e.Name)
                    .HasName("tags_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });
        }
    }
}
