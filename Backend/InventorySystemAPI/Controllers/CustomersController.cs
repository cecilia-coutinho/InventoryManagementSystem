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
    public class CustomersController : ControllerBase
    {
        public readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // GET: api/Customers?pageSize=10&pageNumber=1&filterOn=FirstName&filterQuery=John&sortBy=FirstName&isDescending=true
        [HttpGet]
        public async Task<IActionResult> GetCustomers(
            [FromQuery] int pageSize = 1000,
            [FromQuery] int pageNumber = 1,
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            try
            {
                var (result, totalRecordCount, totalPages, pageNumberMessage, isPrevious, isNext) = await _customerRepository.SearchSortAndPaginationAsync(
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

        //GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                return Ok(customer);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //POST: api/Customers
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDto customerDto)
        {
            try
            {
                var customer = new Customer
                {
                    FkContactId = customerDto.FkContactId,
                    CustomerName = customerDto.CustomerName,
                    ShippingAddress = customerDto.ShippingAddress,
                    City = customerDto.City,
                    PostalCode = customerDto.PostalCode,
                    Country = customerDto.Country
                };

                var newCustomer = await _customerRepository.CreateAsync(customer);
                return CreatedAtAction(nameof(GetCustomer), new { id = newCustomer.Id }, newCustomer);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //PUT: api/Customers/5
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] CustomerCreateDto customerDto)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(id);

            if (existingCustomer == null)
            {
                return NotFound("Customer not found.");
            }

            existingCustomer.FkContactId = customerDto.FkContactId;
            existingCustomer.CustomerName = customerDto.CustomerName;
            existingCustomer.ShippingAddress = customerDto.ShippingAddress;
            existingCustomer.City = customerDto.City;
            existingCustomer.PostalCode = customerDto.PostalCode;
            existingCustomer.Country = customerDto.Country;

            await _customerRepository.UpdateAsync(existingCustomer);
            return Ok(existingCustomer);
        }

        //DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            await _customerRepository.DeleteAsync(customer);
            return Ok(new { message = "Customer deleted successfully.", customer });
        }
    }
}
