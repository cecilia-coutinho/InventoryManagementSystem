namespace InventorySystemAPI.DTOs
{
    public class SupplierReadDto
    {
        public string? SupplierName { get; set; }
        public Guid FKContactId { get; set; }
        public string? ContactName { get; set; }
        public string? ContactEmail { get; set; }
        public string? SupplierAddress { get; set; }
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
