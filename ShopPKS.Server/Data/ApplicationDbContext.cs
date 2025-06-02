using Microsoft.EntityFrameworkCore;
using ShopPKS.Server.Models;

namespace ShopPKS.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
} 