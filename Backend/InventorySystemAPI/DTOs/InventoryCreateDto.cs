using System.ComponentModel.DataAnnotations;

namespace InventorySystemAPI.DTOs
{
    public class InventoryCreateDto
    {
        [Required(ErrorMessage = "Product is required")]
        public Guid FkProductId { get; set; }

        [Required(ErrorMessage = "Quantity in Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity in Stock must be a positive number.")]
        public int QuantityInStock { get; set; }

        [Required(ErrorMessage = "Minimum Stock Level is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimum Stock Level must be a positive number.")]
        public int MinStockLevel { get; set; }

        [Required(ErrorMessage = "Maximum Stock Level is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Maximum Stock Level must be a positive number.")]
        public int MaxStockLevel { get; set; }
    }
}
