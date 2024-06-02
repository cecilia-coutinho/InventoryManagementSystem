using InventorySystemAPI.Data;
using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories.GenericRepository;
using System.Linq.Expressions;

namespace InventorySystemAPI.Repositories
{
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        private new readonly AppDbContext _context;
        public SupplierRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(ICollection<Supplier> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)> SearchSortAndPaginationAsync(
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isDescending,
            int pageNumber,
            int pageSize)
        {
            //search predicate based on filter parameters
            Expression<Func<Supplier, bool>>? searchPredicate = null;
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.ToUpperInvariant())
                {
                    case "SUPPLIERNAME":
                        searchPredicate = s => !string.IsNullOrEmpty(s.SupplierName) && s.SupplierName.Contains(filterQuery);
                        break;
                    default:
                        throw new ArgumentException("Invalid filterOn value.");
                }
            }

            // Order by expression based on sortBy parameter
            Expression<Func<Supplier, object>>? orderBy = null;
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToUpperInvariant())
                {
                    case "SUPPLIERNAME":
                        orderBy = s => !string.IsNullOrEmpty(s.SupplierName) ? s.SupplierName : "";
                        break;
                    default:
                        throw new ArgumentException("Invalid sortBy value.");
                }
            }

            return await base.SearchSortAndPaginationAsync(searchPredicate, orderBy, isDescending, pageNumber, pageSize);
        }
    }
}
