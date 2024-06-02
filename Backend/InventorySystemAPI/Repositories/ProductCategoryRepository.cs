using InventorySystemAPI.Data;
using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories.GenericRepository;

namespace InventorySystemAPI.Repositories
{
    public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
    {
        private new readonly AppDbContext _context;

        public ProductCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
