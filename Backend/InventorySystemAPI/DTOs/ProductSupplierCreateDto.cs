using System.ComponentModel.DataAnnotations;

namespace InventorySystemAPI.DTOs
{
    public class ProductSupplierCreateDto
    {
        [Required(ErrorMessage = "Product Id is required")]
        public Guid FkProductId { get; set; }

        [Required(ErrorMessage = "Supplier Id is required")]
        public Guid FkSupplierId { get; set; }

    }
}
