using System.ComponentModel.DataAnnotations;

namespace ShopPKS.Client.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название товара обязательно")]
        [StringLength(100, ErrorMessage = "Название товара не должно превышать 100 символов")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Цена товара обязательна")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        public string Description { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Количество не может быть отрицательным")]
        public int StockQuantity { get; set; }
    }
} 