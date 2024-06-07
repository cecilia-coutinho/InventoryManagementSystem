using InventorySystemAPI.Data;
using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace InventorySystemAPI.Repositories
{
    public class ProductSupplierRepository : GenericRepository<ProductSupplier>, IProductSupplierRepository
    {
        private new readonly AppDbContext _context;

        public ProductSupplierRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<ProductSupplier>> GetAllAsync()
        {
            if (_context.ProductSuppliers == null || _context.Products == null || _context.Suppliers == null)
            {
                throw new Exception("No data found.");
            }

            var result = await (from ps in _context.ProductSuppliers
                                join p in _context.Products on ps.FkProductId equals p.Id
                                join s in _context.Suppliers on ps.FkSupplierId equals s.Id
                                select new ProductSupplier
                                {
                                    Id = ps.Id,
                                    FkProductId = ps.FkProductId,
                                    Product = p,
                                    FkSupplierId = ps.FkSupplierId,
                                    Supplier = s,
                                    CreatedAt = ps.CreatedAt,
                                    UpdatedAt = ps.UpdatedAt
                                }).ToListAsync();

            return result;
        }

        public override async Task<ProductSupplier> GetByIdAsync(Guid id)
        {
            if (_context.ProductSuppliers == null || _context.Products == null || _context.Suppliers == null)
            {
                throw new Exception("No data found.");
            }

            var result = await (from ps in _context.ProductSuppliers
                                join p in _context.Products on ps.FkProductId equals p.Id
                                join s in _context.Suppliers on ps.FkSupplierId equals s.Id
                                where ps.Id == id
                                select new ProductSupplier
                                {
                                    Id = ps.Id,
                                    FkProductId = ps.FkProductId,
                                    Product = p,
                                    FkSupplierId = ps.FkSupplierId,
                                    Supplier = s,
                                    CreatedAt = ps.CreatedAt,
                                    UpdatedAt = ps.UpdatedAt
                                }).FirstOrDefaultAsync();

            if (result == null)
            {
                throw new Exception("Entity not found.");
            }

            return result;
        }
    }
}
