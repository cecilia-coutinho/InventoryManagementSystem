using InventorySystemAPI.Data;
using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories.GenericRepository;
using InventorySystemAPI.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;

namespace InventorySystemAPI.Repositories
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        private new readonly AppDbContext _context;
        public ContactRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(ICollection<Contact> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)> SearchSortAndPaginationAsync(
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isDescending,
            int pageNumber,
            int pageSize)
        {
            // search predicate based on filter parameters
            Expression<Func<Contact, bool>>? searchPredicate = null;
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.ToUpperInvariant())
                {
                    case "FIRSTNAME":
                        searchPredicate = c => !string.IsNullOrEmpty(c.FirstName) && c.FirstName.Contains(filterQuery);
                        break;
                    case "LASTNAME":
                        searchPredicate = c => !string.IsNullOrEmpty(c.LastName) && c.LastName.Contains(filterQuery);
                        break;
                    case "EMAIL":
                        searchPredicate = c => !string.IsNullOrEmpty(c.Email) && c.Email.Contains(filterQuery);
                        break;
                    default:
                        throw new ArgumentException("Invalid filterOn value.");
                }
            }

            // Order by expression based on sortBy parameter
            Expression<Func<Contact, object>>? orderBy = null;
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToUpperInvariant())
                {
                    case "FIRSTNAME":
                        orderBy = c => !string.IsNullOrEmpty(c.FirstName) ? c.FirstName : "";
                        break;
                    case "LASTNAME":
                        orderBy = c => !string.IsNullOrEmpty(c.LastName) ? c.LastName : "";
                        break;
                    case "EMAIL":
                        orderBy = c => !string.IsNullOrEmpty(c.Email) ? c.Email : "";
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

        private IQueryable<Contact> ApplySpecificationforList(ISpecification<Contact>? spec)
        {
            IQueryable<Contact> query = _context.Set<Contact>();

            if (spec != null)
            {
                query = SpecificationEvaluator<Contact>.GetQuery(query, spec);
            }

            return query;
        }

       public override async Task<Contact?> GetEntityWithSpecAsync(ISpecification<Contact>? specification = null)
        {
            var query = ApplySpecificationforList(specification);
            var result = await query.FirstOrDefaultAsync();
            return result;
        }
    }
}
