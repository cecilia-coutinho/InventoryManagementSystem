using System.ComponentModel.DataAnnotations;

namespace InventorySystemAPI.DTOs
{
    public class ProductCategoryCreateDto
    {
        [Required(ErrorMessage = "Product Category Name is required")]
        [StringLength(30, ErrorMessage = "Product Category name cannot be longer than 30 characters.")]
        public string? ProductCategoryName { get; set; }
    }
}
