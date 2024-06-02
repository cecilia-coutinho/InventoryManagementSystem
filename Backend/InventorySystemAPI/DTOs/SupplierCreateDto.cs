using System.ComponentModel.DataAnnotations;

namespace InventorySystemAPI.DTOs
{
    public class SupplierCreateDto
    {
        [Required(ErrorMessage = "Supplier Name is required")]
        [StringLength(100, ErrorMessage = "Supplier name cannot be longer than 100 characters.")]
        public string? SupplierName { get; set; }

        [Required(ErrorMessage = "Contact Id is required")]
        public Guid FKContactId { get; set; }
        public string? SupplierAddress { get; set; }
    }
}
