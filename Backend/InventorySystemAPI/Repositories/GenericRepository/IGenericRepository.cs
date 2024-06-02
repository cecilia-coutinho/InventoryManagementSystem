using InventorySystemAPI.Specifications;
using System.Linq.Expressions;

namespace InventorySystemAPI.Repositories.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task<(ICollection<T> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)> SearchSortAndPaginationAsync(
            Expression<Func<T, bool>>? searchPredicate = null,
            Expression<Func<T, object>>? orderBy = null,
            bool isDescending = false,
            int? pageNumber = null,
            int? pageSize = null,
            params Expression<Func<T, object>>[]? includeProperties);

        Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T>? specification = null);
        Task<T?> GetEntityWithSpecAsync(ISpecification<T>? specification = null);
    }
}
