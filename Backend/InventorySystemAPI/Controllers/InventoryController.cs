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
    public class InventoryController : ControllerBase
    {
        public readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        // GET: api/Inventory?pageSize=10&pageNumber=1&filterOn=ProductName&filterQuery=Product&sortBy=ProductName&isDescending=true
        [HttpGet]
        public async Task<IActionResult> GetInventory(
            [FromQuery] int pageSize = 1000,
            [FromQuery] int pageNumber = 1,
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            try
            {
                var (result, totalRecordCount, totalPages, pageNumberMessage, isPrevious, isNext) = await _inventoryRepository.SearchSortAndPaginationAsync(
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

        // GET: api/Inventory/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventory(Guid id)
        {
            try
            {
                var inventory = await _inventoryRepository.GetByIdAsync(id);

                if (inventory == null)
                {
                    return NotFound("No data found.");
                }

                return Ok(inventory);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Inventory
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryCreateDto inventoryDto)
        {
            try
            {
                var inventory = new Inventory
                {
                    FkProductId = inventoryDto.FkProductId,
                    QuantityInStock = inventoryDto.QuantityInStock,
                    MinStockLevel = inventoryDto.MinStockLevel,
                    MaxStockLevel = inventoryDto.MaxStockLevel
                };

                var newInventory = await _inventoryRepository.CreateAsync(inventory);
                return CreatedAtAction(nameof(GetInventory), new { id = newInventory.Id }, newInventory);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Inventory/5
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateInventory(Guid id, [FromBody] InventoryCreateDto inventoryDto)
        {
            var existingInventory = await _inventoryRepository.GetByIdAsync(id);

            if (existingInventory == null)
            {
                return NotFound("No data found.");
            }

            existingInventory.FkProductId = inventoryDto.FkProductId;
            existingInventory.QuantityInStock = inventoryDto.QuantityInStock;
            existingInventory.MinStockLevel = inventoryDto.MinStockLevel;
            existingInventory.MaxStockLevel = inventoryDto.MaxStockLevel;

            await _inventoryRepository.UpdateAsync(existingInventory);
            return Ok(existingInventory);
        }

        // DELETE: api/Inventory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(Guid id)
        {
            var inventory = await _inventoryRepository.GetByIdAsync(id);

            if (inventory == null)
            {
                return NotFound("No data found.");
            }

            await _inventoryRepository.DeleteAsync(inventory);
            return Ok(new { message = "Data deleted successfully.", inventory });
        }
    }
}
