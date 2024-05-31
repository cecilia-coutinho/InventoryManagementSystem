using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventorySystemAPI.Models
{
    public class Product : BaseEntity
    {
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public Guid FkProductCategory { get; set; }
        public decimal SellPrice { get; set; }
        public decimal CostPrice { get; set; }

        [JsonIgnore]
        public virtual ProductCategory? ProductCategories { get; set; }

        [JsonIgnore]
        public virtual Inventory? Inventory { get; set; }

        [JsonIgnore]
        public ICollection<OrderDetail>? OrderDetails { get; set; }

        [JsonIgnore]
        public ICollection<ProductSupplier>? productSuppliers { get; set; }
    }
}
