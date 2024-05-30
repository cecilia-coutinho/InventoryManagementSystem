﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventorySystemAPI.Models
{
    public class ProductSupplier
    {
        [Key]
        public Guid ProductSupplierId { get; set; }
        public Guid FkProductId { get; set; }
        public Guid FkSupplierId { get; set; }

        [JsonIgnore]
        public virtual Product? Product { get; set; }

        [JsonIgnore]
        public virtual Supplier? Supplier { get; set; }
    }
}
