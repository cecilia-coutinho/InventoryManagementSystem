using InventorySystemAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventorySystemAPI.Models
{
    public class Order : BaseEntity
    {
        public Guid? FkCustomerId { get; set; }
        public Guid? FkSupplierID { get; set; }
        public OrderType OrderType { get; set; }
        public DateTime OrderDate { get; set; }

        [JsonIgnore]
        public virtual Supplier? Suppliers { get; set; }

        [JsonIgnore]
        public virtual Customer? Customers { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
