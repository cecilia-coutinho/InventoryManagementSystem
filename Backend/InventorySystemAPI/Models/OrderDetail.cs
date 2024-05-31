using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventorySystemAPI.Models
{
    public class OrderDetail : BaseEntity
    {
        public Guid FkOrderId { get; set; }
        public Guid FkProductId { get; set; }
        public int OrderQuantity { get; set; }

        [JsonIgnore]
        public virtual Order? Order { get; set; }

        [JsonIgnore]
        public virtual Product? Product { get; set; }
    }
}
