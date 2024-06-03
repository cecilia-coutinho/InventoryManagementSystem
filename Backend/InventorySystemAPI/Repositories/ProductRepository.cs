using InventorySystemAPI.Data;
using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventorySystemAPI.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private new readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(ICollection<Product> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)> SearchSortAndPaginationAsync(string? filterOn, string? filterQuery, string? sortBy, bool isDescending, int pageNumber, int pageSize)
        {
            // search predicate based on filter parameters
            Expression<Func<Product, bool>>? searchPredicate = null;

            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.ToUpperInvariant())
                {
                    case "PRODUCTNAME":
                        searchPredicate = p => !string.IsNullOrEmpty(p.ProductName) && p.ProductName.Contains(filterQuery);
                        break;
                    case "CATEGORY":
                        if (_context.ProductCategories == null)
                        {
                            throw new ArgumentException("Product categories not found.");
                        }

                        var categoryId = _context.ProductCategories
                                                .Where(pc => pc.ProductCategoryName != null && pc.ProductCategoryName.Contains(filterQuery))
                                                .Select(pc => pc.Id)
                                                .FirstOrDefault();

                        // category ID to filter products
                        searchPredicate = p => p.FkProductCategory == categoryId;
                        break;
                    case "DESCRIPTION":
                        searchPredicate = p => !string.IsNullOrEmpty(p.ProductDescription) && p.ProductDescription.Contains(filterQuery);
                        break;
                    case "SELLPRICE":
                        searchPredicate = GetPricePredicate(p => p.SellPrice, filterQuery);
                        break;
                    case "COSTPRICE":
                        searchPredicate = GetPricePredicate(p => p.CostPrice, filterQuery);
                        break;
                    default:
                        throw new ArgumentException("Invalid filterOn value.");
                }
            }

            // Order by expression based on sortBy parameter
            Expression<Func<Product, object>>? orderBy = null;

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToUpperInvariant())
                {
                    case "PRODUCTNAME":
                        orderBy = p => !string.IsNullOrEmpty(p.ProductName) ? p.ProductName : "";
                        break;
                    case "CATEGORY":
                        orderBy = p => p.ProductCategories != null && p.ProductCategories.ProductCategoryName != null
                        ? p.ProductCategories.ProductCategoryName
                        : "";
                        break;
                    case "DESCRIPTION":
                        orderBy = p => !string.IsNullOrEmpty(p.ProductDescription) ? p.ProductDescription : "";
                        break;
                    case "SELLPRICE":
                        orderBy = p => p.SellPrice;
                        break;
                    case "COSTPRICE":
                        orderBy = p => p.CostPrice;
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

        private Expression<Func<Product, bool>> GetPricePredicate(Expression<Func<Product, decimal>> priceSelector, string filterQuery)
        {
            var parameter = priceSelector.Parameters[0];
            var property = (MemberExpression)priceSelector.Body;

            if (filterQuery.StartsWith('-'))
            {
                throw new ArgumentException($"Invalid filter query for price: '{filterQuery}'. It cannot be < 0", nameof(filterQuery));
            }
            else if (filterQuery.Contains('-'))
            {
                var bounds = filterQuery.Split('-');
                if (bounds.Length == 2
                    && decimal.TryParse(bounds[0], out decimal min)
                    && decimal.TryParse(bounds[1], out decimal max))
                {
                    var minExpression = Expression.GreaterThanOrEqual(property, Expression.Constant(min));
                    var maxExpression = Expression.LessThanOrEqual(property, Expression.Constant(max));
                    var andExpression = Expression.AndAlso(minExpression, maxExpression);
                    return Expression.Lambda<Func<Product, bool>>(andExpression, parameter);
                }
            }
            else if (decimal.TryParse(filterQuery, out decimal amount))
            {
                var equalExpression = Expression.Equal(property, Expression.Constant(amount));
                return Expression.Lambda<Func<Product, bool>>(equalExpression, parameter);
            }
            else
            {
                throw new ArgumentException($"Invalid filter query for price: '{filterQuery}'", nameof(filterQuery));
            }

            throw new ArgumentException($"Invalid filter query for price: '{filterQuery}'", nameof(filterQuery));
        }
    }
}
