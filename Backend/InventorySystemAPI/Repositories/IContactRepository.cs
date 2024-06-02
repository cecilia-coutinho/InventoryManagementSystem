using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories.GenericRepository;
using InventorySystemAPI.Specifications;

namespace InventorySystemAPI.Repositories
{
    public interface IContactRepository : IGenericRepository<Contact>
    {
        Task<(ICollection<Contact> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)> SearchSortAndPaginationAsync(
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isDescending,
            int pageNumber,
            int pageSize);

        new Task<Contact?> GetEntityWithSpecAsync(ISpecification<Contact>? specification = null);
    }
}
