using InventorySystemAPI.Data;
using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
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

        public override async Task<Supplier> GetByIdAsync(Guid id)
        {
            if (_context.Suppliers == null || _context.Contacts == null)
            {
                throw new Exception("No data found.");
            }

            var result = await (from s in _context.Suppliers
                                join c in _context.Contacts on s.FKContactId equals c.Id
                                where s.Id == id
                                select new Supplier
                                {
                                    Id = s.Id,
                                    SupplierName = s.SupplierName,
                                    FKContactId = s.FKContactId,
                                    Contact = c,
                                    SupplierAddress = s.SupplierAddress,
                                    CreatedAt = s.CreatedAt,
                                    UpdatedAt = s.UpdatedAt
                                }).FirstOrDefaultAsync();

            if (result == null) {
                throw new Exception("Entity not found.");
            }

            return result;
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

            // Call the base method with searchPredicate and orderBy
            var (result, totalRecordCount, totalPages, pageNumberMessage, isPrevious, isNext) = await base.SearchSortAndPaginationAsync(searchPredicate, orderBy, isDescending, pageNumber, pageSize);

            if (_context.Contacts == null)
            {
                throw new Exception("No data found.");
            }

            // Ensure Contacts load for each supplier
            foreach (var supplier in result)
            {
                supplier.Contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == supplier.FKContactId);
            }

            return (result, totalRecordCount, totalPages, pageNumberMessage, isPrevious, isNext);
        }
    }
}
