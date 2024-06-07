namespace InventorySystemAPI.DTOs
{
    public class ProductSupplierReadDto
    {
        public Guid FkProductId { get; set; }
        public string? ProductName { get; set; }
        public Guid FkSupplierId { get; set; }
        public string? SupplierName { get; set; }
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
