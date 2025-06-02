using System.ComponentModel.DataAnnotations;

namespace ShopPKS.Server.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public int StockQuantity { get; set; }
    }
} 