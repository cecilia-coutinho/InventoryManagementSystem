using InventorySystemAPI.Data;
using InventorySystemAPI.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventorySystemAPI.Repositories.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity == null)
            {
                throw new Exception("Entity not found");
            }

            return entity;
        }

        public async Task<T> CreateAsync(T entity)
        {
            var result = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<(ICollection<T> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)> SearchSortAndPaginationAsync(
            Expression<Func<T, bool>>? searchPredicate = null,
            Expression<Func<T, object>>? orderBy = null,
            bool isDescending = false,
            int? pageNumber = null,
            int? pageSize = null,
            params Expression<Func<T, object>>[]? includeProperties)
        {
            pageNumber ??= 1; //Default value
            pageNumber--; // 0 based index

            if (pageNumber < 0)
            {
                throw new ArgumentException("Page number cannot be less than 1.");
            }

            pageSize ??= 10;

            IQueryable<T> query = _context.Set<T>();

            if (searchPredicate != null)
            {
                query = query.Where(searchPredicate);
            }

            if (orderBy != null)
            {
                query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }

            foreach (var includeProperty in includeProperties ?? Enumerable.Empty<Expression<Func<T, object>>>())
            {
                query = query.Include(includeProperty);
            }


            var totalRecordCount = await query.CountAsync();

            int? totalPages = pageNumber.HasValue && pageSize.HasValue
               ? (int?)Math.Ceiling((double)totalRecordCount / pageSize.Value)
               : null;

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = query.Skip(pageNumber.Value * pageSize.Value)
                             .Take(pageSize.Value);
            }

            bool? isPrevious = pageNumber.HasValue ? pageNumber > 0 : null;
            bool? isNext = pageNumber.HasValue && totalPages.HasValue ? pageNumber + 1 < totalPages : null;

            int actualPageNumber = pageNumber.GetValueOrDefault(0) + 1;
            string pageIndexMessage = totalPages.HasValue ? $"Page {actualPageNumber} from {totalPages} pages" : "";

            ICollection<T> result = await query.ToListAsync();

            return (result, totalRecordCount, totalPages ?? 0, pageIndexMessage, isPrevious ?? false, isNext ?? false);
        }

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T>? specification = null)
        {
            return await ApplySpecificationforList(specification).ToListAsync();
        }

        private IQueryable<T> ApplySpecificationforList(ISpecification<T>? spec)
        {
            IQueryable<T> query = _context.Set<T>();

            if (spec != null)
            {
                query = SpecificationEvaluator<T>.GetQuery(query, spec);
            }

            return query;
        }

        public virtual async Task<T?> GetEntityWithSpecAsync(ISpecification<T>? specification = null)
        {
            var query = ApplySpecificationforList(specification);
            var result = await query.FirstOrDefaultAsync();
            if (result == null)
            {
                throw new Exception("Entity not found with given specification");
            }

            return result;
        }
    }
}
