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
    public class ProductsController : ControllerBase
    {
        public readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/Products?pageSize=10&pageNumber=1&filterOn=ProductName&filterQuery=Product&sortBy=ProductName&isDescending=true
        [HttpGet]
        public async Task<IActionResult> GetProducts(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1,
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            try
            {
                var (result, totalRecordCount, totalPages, pageNumberMessage, isPrevious, isNext) = await _productRepository.SearchSortAndPaginationAsync(
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

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);

                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Products
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productDto)
        {
            try
            {
                var product = new Product
                {
                    ProductName = productDto.ProductName,
                    ProductDescription = productDto.ProductDescription,
                    FkProductCategory = productDto.FkProductCategory,
                    SellPrice = productDto.SellPrice,
                    CostPrice = productDto.CostPrice
                };

                var newProduct = await _productRepository.CreateAsync(product);
                return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductCreateDto productDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);

            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }

            existingProduct.ProductName = productDto.ProductName;
            existingProduct.ProductDescription = productDto.ProductDescription;
            existingProduct.FkProductCategory = productDto.FkProductCategory;
            existingProduct.SellPrice = productDto.SellPrice;
            existingProduct.CostPrice = productDto.CostPrice;

            await _productRepository.UpdateAsync(existingProduct);
            return Ok(existingProduct);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            await _productRepository.DeleteAsync(product);
            return Ok(new { message = "Product deleted successfully.", product });
        }
    }
}
