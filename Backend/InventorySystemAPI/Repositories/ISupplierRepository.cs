using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories.GenericRepository;

namespace InventorySystemAPI.Repositories
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        Task<(ICollection<Supplier> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)> SearchSortAndPaginationAsync(
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isDescending,
            int pageNumber,
            int pageSize);
    }
}
