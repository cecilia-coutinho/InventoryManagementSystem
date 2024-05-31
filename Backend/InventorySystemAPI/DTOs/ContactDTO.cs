using System.ComponentModel.DataAnnotations;

namespace InventorySystemAPI.DTOs
{
    public class ContactDTO
    {
        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
