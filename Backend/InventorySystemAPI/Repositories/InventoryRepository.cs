using InventorySystemAPI.Data;
using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories.GenericRepository;
using System.Linq.Expressions;

namespace InventorySystemAPI.Repositories
{
    public class InventoryRepository : GenericRepository<Inventory>, IInventoryRepository
    {
        private new readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<(ICollection<Inventory> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)> SearchSortAndPaginationAsync(string? filterOn, string? filterQuery, string? sortBy, bool isDescending, int pageNumber, int pageSize)
        {
            // search predicate based on filter parameters
            Expression<Func<Inventory, bool>>? searchPredicate = null;

            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.ToUpperInvariant())
                {
                    case "PRODUCTNAME":
                        if (_context.Products == null)
                        {
                            throw new ArgumentException("Products not found.");
                        }

                        var productId = _context.Products
                                                .Where(p => p.ProductName != null && p.ProductName.Contains(filterQuery))
                                                .Select(p => p.Id)
                                                .FirstOrDefault();

                        // product ID to filter inventory
                        searchPredicate = i => i.FkProductId == productId;
                        break;
                    case "QUANTITY":
                        searchPredicate = GetQuantityPredicate(i => i.QuantityInStock, filterQuery);
                        break;
                    case "MINSTOCKLEVEL":
                        searchPredicate = GetQuantityPredicate(i => i.MinStockLevel, filterQuery);
                        break;
                    case "MAXSTOCKLEVEL":
                        searchPredicate = GetQuantityPredicate(i => i.MaxStockLevel, filterQuery);
                        break;
                    default:
                        throw new ArgumentException("Invalid filterOn value.");
                }
            }

            // Order by expression based on sortBy parameter
            Expression<Func<Inventory, object>>? orderBy = null;

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToUpperInvariant())
                {
                    case "PRODUCTNAME":
                        orderBy = i => i.Products != null && i.Products.ProductName != null ? i.Products.ProductName : "";
                        break;
                    case "QUANTITY":
                        orderBy = i => i.QuantityInStock;
                        break;
                    case "MINSTOCKLEVEL":
                        orderBy = i => i.MinStockLevel;
                        break;
                    case "MAXSTOCKLEVEL":
                        orderBy = i => i.MaxStockLevel;
                        break;
                    default:
                        throw new ArgumentException("Invalid sortBy value.");
                }
            }

            return base.SearchSortAndPaginationAsync(
                searchPredicate,
                orderBy,
                isDescending,
                pageNumber,
                pageSize
            );
        }

        private Expression<Func<Inventory, bool>> GetQuantityPredicate(Expression<Func<Inventory, int>> quantityProperty, string filterQuery)
        {
            if (!string.IsNullOrEmpty(filterQuery) && int.TryParse(filterQuery, out int quantity))
            {
                var parameter = Expression.Parameter(typeof(Inventory), "i");
                var propertyAccess = Expression.Invoke(quantityProperty, parameter);
                var constant = Expression.Constant(quantity);
                var body = Expression.Equal(propertyAccess, constant);
                var predicate = Expression.Lambda<Func<Inventory, bool>>(body, parameter);

                return predicate;
            }

            throw new ArgumentException("Invalid quantity value.");
        }
    }
}
