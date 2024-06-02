using InventorySystemAPI.CustomActionFilters;
using InventorySystemAPI.DTOs;
using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {

        public readonly ISupplierRepository _supplierRepository;

        public SuppliersController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        // GET: api/Suppliers?pageSize=10&pageNumber=1&filterOn=SupplierName&filterQuery=John&sortBy=SupplierName&isDescending=true
        [HttpGet]
        public async Task<IActionResult> GetSuppliers(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1,
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            try
            {
                var (result, totalRecordCount, totalPages, pageNumberMessage, isPrevious, isNext) = await _supplierRepository.SearchSortAndPaginationAsync(
                    filterOn,
                    filterQuery,
                    sortBy,
                    isDescending,
                    pageNumber,
                    pageSize);

                if (result == null || !result.Any())
                {
                    return NotFound("No data found.");
                }

                return Ok(new
                {
                    Result = result,
                    TotalRecordCount = totalRecordCount,
                    TotalPages = totalPages,
                    PageNumberMessage = pageNumberMessage,
                    IsPrevious = isPrevious,
                    IsNext = isNext
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);

            }
        }

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplier(Guid id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound("Supplier not found.");
            }

            return Ok(supplier);
        }

        // POST: api/Suppliers
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierCreateDto supplierDto)
        {
            try
            {
                var supplier = new Supplier
                {
                    SupplierName = supplierDto.SupplierName,
                    FKContactId = supplierDto.FKContactId,
                    SupplierAddress = supplierDto.SupplierAddress
                };

                var newSupplier = await _supplierRepository.CreateAsync(supplier);
                return CreatedAtAction(nameof(GetSupplier), new { id = newSupplier.Id }, newSupplier);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Suppliers/5
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateSupplier(Guid id, [FromBody] SupplierCreateDto supplierDto)
        {
            var existingSupplier = await _supplierRepository.GetByIdAsync(id);

            if (existingSupplier == null)
            {
                return NotFound("Supplier not found.");
            }

            existingSupplier.SupplierName = supplierDto.SupplierName;
            existingSupplier.FKContactId = supplierDto.FKContactId;
            existingSupplier.SupplierAddress = supplierDto.SupplierAddress;

            await _supplierRepository.UpdateAsync(existingSupplier);
            return Ok(existingSupplier);
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(Guid id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound("Supplier not found.");
            }

            await _supplierRepository.DeleteAsync(supplier);
            return Ok(new { message = "Supplier deleted successfully.", supplier });
        }
    }
}

