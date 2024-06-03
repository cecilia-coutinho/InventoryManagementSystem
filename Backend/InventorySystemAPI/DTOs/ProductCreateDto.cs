using System.ComponentModel.DataAnnotations;

namespace InventorySystemAPI.DTOs
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot be longer than 100 characters.")]
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }

        [Required(ErrorMessage = "Product Category is required")]
        public Guid FkProductCategory { get; set; }

        [Required(ErrorMessage = "Sell Price is required")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Sell Price must be a positive number.")]
        [DataType(DataType.Currency, ErrorMessage = "Invalid format for Sell Price.")]
        public decimal SellPrice { get; set; }

        [Required(ErrorMessage = "Cost Price is required")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Cost Price must be a positive number.")]
        [DataType(DataType.Currency, ErrorMessage = "Invalid format for Cost Price.")]
        public decimal CostPrice { get; set; }
    }
}
