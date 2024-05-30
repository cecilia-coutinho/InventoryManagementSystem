using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventorySystemAPI.Models
{
    public class Supplier
    {
        [Key]
        public Guid SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public Guid FKContactId { get; set; }
        public string? SupplierAddress { get; set; }

        [JsonIgnore]
        public virtual Contact? Contact { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProductSupplier>? ProductSuppliers { get; set; }
    }
}
