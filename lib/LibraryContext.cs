using System;
using Microsoft.EntityFrameworkCore;

namespace lib
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Подключение к PostgreSQL
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5433;Database=librarydb;Username=postgres;Password=danielDaniel1907!;Include Error Detail=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка таблиц
            modelBuilder.Entity<Book>().ToTable("books");
            modelBuilder.Entity<Author>().ToTable("authors");
            modelBuilder.Entity<Genre>().ToTable("genres");

            // Настройка связей
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author) // Книга имеет одного автора
                .WithMany(a => a.Books) // У автора может быть много книг
                .HasForeignKey(b => b.AuthorId) // Внешний ключ AuthorId
                .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Genre) // Книга относится к одному жанру
                .WithMany(g => g.Books) // У жанра может быть много книг
                .HasForeignKey(b => b.GenreId) // Внешний ключ GenreId
                .OnDelete(DeleteBehavior.SetNull); // При удалении жанра GenreId = NULL

            // Настройка полей для таблицы Book
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id); // Id — первичный ключ
                entity.Property(b => b.Id).ValueGeneratedOnAdd(); // Автоматическая генерация
                entity.Property(b => b.Title).IsRequired().HasMaxLength(255);
                entity.Property(b => b.AuthorId).IsRequired();
                entity.Property(b => b.PublishYear);
                entity.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
                entity.Property(b => b.GenreId);
                entity.Property(b => b.QuantityInStock).HasDefaultValue(0);
            });

            // Настройка полей для таблицы Author
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.Id); // Id — первичный ключ
                entity.Property(a => a.Id).ValueGeneratedOnAdd(); // Автоматическая генерация
                entity.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.LastName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.BirthDate).HasColumnType("date");
                entity.Property(a => a.Country).HasMaxLength(100);
            });

            // Настройка полей для таблицы Genre
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(g => g.Id); // Id — первичный ключ
                entity.Property(g => g.Id).ValueGeneratedOnAdd(); // Автоматическая генерация
                entity.Property(g => g.Name).IsRequired().HasMaxLength(100);
                entity.Property(g => g.Description).HasColumnType("text");
            });
        }
    }
}