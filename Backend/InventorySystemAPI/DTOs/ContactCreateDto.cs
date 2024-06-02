using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InventorySystemAPI.DTOs
{
    public class ContactCreateDto
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First name can not be longer than 50 characteres")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(100, ErrorMessage = "Last Name can not be longer than 100 characteres")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email address")]
        public string? Email { get; set; }
    }
}
