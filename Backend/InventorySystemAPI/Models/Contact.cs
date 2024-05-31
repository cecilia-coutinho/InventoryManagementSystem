using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventorySystemAPI.Models
{
    public class Contact : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        [JsonIgnore]
        public ICollection<Customer>? Customers { get; set; }

        [JsonIgnore]
        public ICollection<Supplier>? Suppliers { get; set; }
    }
}
