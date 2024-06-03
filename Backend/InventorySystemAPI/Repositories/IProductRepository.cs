using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories.GenericRepository;

namespace InventorySystemAPI.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<(ICollection<Product> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)> SearchSortAndPaginationAsync(
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isDescending,
            int pageNumber,
            int pageSize);
    }
}
