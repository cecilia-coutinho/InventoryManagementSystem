using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventorySystemAPI.Models
{
    public class Customer : BaseEntity
    {
        public Guid FkContactId { get; set; }
        public string? CustomerName { get; set; }
        public string? ShippingAddress { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

        [JsonIgnore]
        public virtual Contact? Contacts { get; set; }
    }
}
