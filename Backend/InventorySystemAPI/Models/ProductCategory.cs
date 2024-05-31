using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventorySystemAPI.Models
{
    public class ProductCategory : BaseEntity
    {
        public string? ProductCategoryName { get; set; }

        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }
    }
}
