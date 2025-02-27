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
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5433;Database=library;Username=postgres;Password=danielDaniel1907!");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка таблиц
            modelBuilder.Entity<Book>().ToTable("books");
            modelBuilder.Entity<Author>().ToTable("authors");
            modelBuilder.Entity<Genre>().ToTable("genres");

            // Настройка связей
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

            // Настройка полей с маленькой буквы
            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(b => b.Id).HasColumnName("id");
                entity.Property(b => b.Title).HasColumnName("title").IsRequired().HasMaxLength(100);
                entity.Property(b => b.ISBN).HasColumnName("isbn").IsRequired().HasMaxLength(20);
                entity.Property(b => b.AuthorId).HasColumnName("authorid");
                entity.Property(b => b.GenreId).HasColumnName("genreid");
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(a => a.Id).HasColumnName("id");
                entity.Property(a => a.FirstName).HasColumnName("firstname").IsRequired().HasMaxLength(50);
                entity.Property(a => a.LastName).HasColumnName("lastname").IsRequired().HasMaxLength(50);
            });
            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(b => b.PublishYear).HasColumnName("publishyear");
            });
            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(b => b.QuantityInStock).HasColumnName("quantityinstock");
            });
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(b => b.Country).HasColumnName("country");
            });
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(b => b.BirthDate).HasColumnName("birthdate");
            });
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(b => b.Description).HasColumnName("description");
            });
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(g => g.Id).HasColumnName("id");
                entity.Property(g => g.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
            });
        }
    }
}