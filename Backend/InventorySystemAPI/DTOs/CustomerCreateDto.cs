using System.ComponentModel.DataAnnotations;

namespace InventorySystemAPI.DTOs
{
    public class CustomerCreateDto
    {
        [Required(ErrorMessage = "Contact Id is required")]
        public Guid FkContactId { get; set; }

        [Required(ErrorMessage = "Customer Name is required")]
        [StringLength(100, ErrorMessage = "Customer name cannot be longer than 100 characters.")]
        public string? CustomerName { get; set; }
        public string? ShippingAddress { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters.")]
        public string? City { get; set; }

        [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "Invalid Postal Code. Postal Code should be in the format 00000-000 or 00000000.")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(50, ErrorMessage = "Country cannot be longer than 50 characters.")]
        public string? Country { get; set; }
    }
}
