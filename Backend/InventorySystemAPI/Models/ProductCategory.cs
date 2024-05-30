using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventorySystemAPI.Models
{
    public class ProductCategory
    {
        [Key]
        public Guid Id { get; set; }
        public string? ProductCategoryName { get; set; }

        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }
    }
}
