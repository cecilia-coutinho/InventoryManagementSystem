using InventorySystemAPI.CustomActionFilters;
using InventorySystemAPI.DTOs;
using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSuppliersController : ControllerBase
    {
        public readonly IProductSupplierRepository _productSupplierRepository;

        public ProductSuppliersController(IProductSupplierRepository productSupplierRepository)
        {
            _productSupplierRepository = productSupplierRepository;
        }

        // GET: api/ProductSuppliers
        [HttpGet]
        public async Task<IActionResult> GetProductSuppliers()
        {
            try
            {
                var productSuppliers = await _productSupplierRepository.GetAllAsync();

                if (productSuppliers == null || !productSuppliers.Any())
                {
                    return NotFound("No data found.");
                }

                var resultDto = productSuppliers.Select(ps => new ProductSupplierReadDto
                {
                    FkProductId = ps.FkProductId,
                    ProductName = ps.Product?.ProductName,
                    FkSupplierId = ps.FkSupplierId,
                    SupplierName = ps.Supplier?.SupplierName,
                    Id = ps.Id,
                    CreatedAt = ps.CreatedAt,
                    UpdatedAt = ps.UpdatedAt
                });

                return Ok(resultDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/ProductSuppliers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductSupplier(Guid id)
        {
            try
            {
                var productSupplier = await _productSupplierRepository.GetByIdAsync(id);

                if (productSupplier == null)
                {
                    return NotFound("No data found.");
                }

                var resultDto = new ProductSupplierReadDto
                {
                    FkProductId = productSupplier.FkProductId,
                    ProductName = productSupplier.Product?.ProductName,
                    FkSupplierId = productSupplier.FkSupplierId,
                    SupplierName = productSupplier.Supplier?.SupplierName,
                    Id = productSupplier.Id,
                    CreatedAt = productSupplier.CreatedAt,
                    UpdatedAt = productSupplier.UpdatedAt
                };

                return Ok(resultDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/ProductSuppliers
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> PostProductSupplier([FromBody] ProductSupplierCreateDto productSupplierDto)
        {
            try
            {
                var productSupplier = new ProductSupplier
                {
                    FkProductId = productSupplierDto.FkProductId,
                    FkSupplierId = productSupplierDto.FkSupplierId
                };

                var newProductSupplier = await _productSupplierRepository.CreateAsync(productSupplier);
                return CreatedAtAction(nameof(GetProductSupplier), new { id = newProductSupplier.Id }, newProductSupplier);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/ProductSuppliers/5
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> PutProductSupplier(Guid id, [FromBody] ProductSupplierCreateDto productSupplierDto)
        {
            var existingProductSupplier = await _productSupplierRepository.GetByIdAsync(id);

            if (existingProductSupplier == null)
            {
                return NotFound("No data found.");
            }

            existingProductSupplier.FkProductId = productSupplierDto.FkProductId;
            existingProductSupplier.FkSupplierId = productSupplierDto.FkSupplierId;

            await _productSupplierRepository.UpdateAsync(existingProductSupplier);
            return Ok(existingProductSupplier);
        }

        // DELETE: api/ProductSuppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductSupplier(Guid id)
        {
            var productSupplier = await _productSupplierRepository.GetByIdAsync(id);

            if (productSupplier == null)
            {
                return NotFound("No data found.");
            }

            await _productSupplierRepository.DeleteAsync(productSupplier);
            return Ok(new { messahe = "Record deleted successfully.", productSupplier });
        }
    }
}
