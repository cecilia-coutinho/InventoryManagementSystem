using InventorySystemAPI.DTOs;
using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace InventorySystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        public readonly IGenericRepository<Contact> _contactRepository;

        public ContactsController(IGenericRepository<Contact> contactRepository)
        {
            _contactRepository = contactRepository;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            var contacts = await _contactRepository.GetAllAsync();
            return Ok(contacts);
        }

        // GET: api/Contacts/WithFilterAndPagination?pageSize=10&pageIndex=0&filterOn=FirstName&filterQuery=John&sortBy=FirstName&isAscending=true
        [HttpGet("WithFilterAndPagination")]
        public async Task<IActionResult> GetContactsWithFilterAndPagination(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageIndex = 0,
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isAscending = true)
        {
            // Create search predicate based on query parameters
            Expression<Func<Contact, bool>>? searchPredicate = null;
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn?.ToUpperInvariant())
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
                        return BadRequest("Invalid filterOn value.");
                }
            }

            // Call repository method with search predicate
            var (result, totalRecordCount, totalPages, isPrevious, isNext) = await _contactRepository.SearchOrderAndPaginationAsync(
                searchPredicate,
                orderBy: null,
                isDescending: !isAscending,
                pageNumber: pageIndex,
                pageSize: pageSize,
                includeProperties: null);

            if (result == null || !result.Any())
            {
                return NotFound("No data found.");
            }

            // Return the result with pagination information
            return Ok(new
            {
                Result = result,
                TotalRecordCount = totalRecordCount,
                TotalPages = totalPages,
                IsPrevious = isPrevious,
                IsNext = isNext
            });
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact(Guid id)
        {
            var contact = await _contactRepository.GetByIdAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        // POST: api/Contacts
        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] ContactDTO contactDto)
        {
            var contact = new Contact
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                Email = contactDto.Email
            };

            var newContact = await _contactRepository.CreateAsync(contact);
            return CreatedAtAction(nameof(GetContact), new { id = newContact.Id }, newContact);
        }

        // PUT: api/Contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(Guid id, [FromBody] ContactDTO contactDto)
        {
            var existingContact = await _contactRepository.GetByIdAsync(id);

            if (existingContact == null)
            {
                return NotFound();
            }

            existingContact.FirstName = contactDto.FirstName;
            existingContact.LastName = contactDto.LastName;
            existingContact.Email = contactDto.Email;
            existingContact.UpdatedAt = DateTime.Now;

            await _contactRepository.UpdateAsync(existingContact);

            return Ok(existingContact);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            var contact = await _contactRepository.GetByIdAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            await _contactRepository.DeleteAsync(contact);

            return Ok(contact);
        }
    }
}
