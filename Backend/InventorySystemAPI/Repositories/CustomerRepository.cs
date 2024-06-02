using InventorySystemAPI.Data;
using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories.GenericRepository;
using System.Linq.Expressions;

namespace InventorySystemAPI.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private new readonly AppDbContext _context;
        public CustomerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(ICollection<Customer> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)> SearchSortAndPaginationAsync(
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isDescending,
            int pageNumber,
            int pageSize)
        {
            // search predicate based on filter parameters
            Expression<Func<Customer, bool>>? searchPredicate = null;
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.ToUpperInvariant())
                {
                    case "CUSTOMERNAME":
                        searchPredicate = c => !string.IsNullOrEmpty(c.CustomerName) && c.CustomerName.Contains(filterQuery);
                        break;
                    case "CITY":
                        searchPredicate = c => !string.IsNullOrEmpty(c.City) && c.City.Contains(filterQuery);
                        break;
                    case "COUNTRY":
                        searchPredicate = c => !string.IsNullOrEmpty(c.Country) && c.Country.Contains(filterQuery);
                        break;
                    default:
                        throw new ArgumentException("Invalid filterOn value.");
                }
            }

            // Order by expression based on sortBy parameter
            Expression<Func<Customer, object>>? orderBy = null;
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToUpperInvariant())
                {
                    case "CUSTOMERNAME":
                        orderBy = c => !string.IsNullOrEmpty(c.CustomerName) ? c.CustomerName : "";
                        break;
                    case "CITY":
                        orderBy = c => !string.IsNullOrEmpty(c.City) ? c.City : "";
                        break;
                    case "COUNTRY":
                        orderBy = c => !string.IsNullOrEmpty(c.Country) ? c.Country : "";
                        break;
                    default:
                        throw new ArgumentException("Invalid sortBy value.");
                }
            }

            return await base.SearchSortAndPaginationAsync(
                searchPredicate,
                orderBy,
                isDescending,
                pageNumber,
                pageSize
            );
        }
    }
}
