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
    public class ProductCategoriesController : ControllerBase
    {
        public readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoriesController(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        // GET: api/ProductCategories
        [HttpGet]
        public async Task<IActionResult> GetProductCategories()
        {
            try
            {
                var result = await _productCategoryRepository.GetAllAsync();

                if (result == null || !result.Any())
                {
                    return NotFound("No data found.");
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/ProductCategories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductCategory(Guid id)
        {
            try
            {
                var productCategory = await _productCategoryRepository.GetByIdAsync(id);

                if (productCategory == null)
                {
                    return NotFound("No data found.");
                }

                return Ok(productCategory);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/ProductCategories
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateProductCategory([FromBody] ProductCategoryCreateDto productCategoryDto)
        {
            try
            {
                var productCategory = new ProductCategory
                {
                    ProductCategoryName = productCategoryDto.ProductCategoryName
                };

                var newProductCategory = await _productCategoryRepository.CreateAsync(productCategory);
                return CreatedAtAction(nameof(GetProductCategory), new { id = newProductCategory.Id }, newProductCategory);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/ProductCategories/5
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateProductCategory(Guid id, [FromBody] ProductCategoryCreateDto productCategoryDto)
        {

            var existingProductCategory = await _productCategoryRepository.GetByIdAsync(id);

            if (existingProductCategory == null)
            {
                return NotFound("No data found.");
            }

            existingProductCategory.ProductCategoryName = productCategoryDto.ProductCategoryName;

            await _productCategoryRepository.UpdateAsync(existingProductCategory);
            return Ok(existingProductCategory);
        }

        // DELETE: api/ProductCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(Guid id)
        {
            var productCategory = await _productCategoryRepository.GetByIdAsync(id);

            if (productCategory == null)
            {
                return NotFound("No data found.");
            }

            await _productCategoryRepository.DeleteAsync(productCategory);
            return Ok(new { message = "Product category deleted successfully.", productCategory });
        }
    }
}
