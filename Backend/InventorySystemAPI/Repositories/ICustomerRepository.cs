﻿using InventorySystemAPI.Models;

namespace InventorySystemAPI.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<(ICollection<Customer> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)> SearchSortAndPaginationAsync(
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isDescending,
            int pageNumber,
            int pageSize);
    }
}
