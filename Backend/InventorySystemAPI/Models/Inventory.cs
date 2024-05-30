using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventorySystemAPI.Models
{
    public class Inventory
    {
        [Key]
        public Guid InventoryId { get; set; }
        public Guid FkProductId { get; set; }
        public int QuantityInStock { get; set; }
        public int MinStockLevel { get; set; }
        public int MaxStockLevel { get; set; }

        [JsonIgnore]
        public Product? Products { get; set; }
    }
}
