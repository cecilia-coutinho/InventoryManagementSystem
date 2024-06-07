using InventorySystemAPI.CustomActionFilters;
using InventorySystemAPI.DTOs;
using InventorySystemAPI.Models;
using InventorySystemAPI.Repositories;
using InventorySystemAPI.Specifications;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace InventorySystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        public readonly IContactRepository _contactRepository;

        public ContactsController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        // GET: api/Contacts?pageSize=10&pageNumber=1&filterOn=FirstName&filterQuery=John&sortBy=FirstName&isDescending=true
        [HttpGet]
        public async Task<IActionResult> GetContacts(
            [FromQuery] int pageSize = 1000,
            [FromQuery] int pageNumber = 1,
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            try
            {
                var (result, totalRecordCount, totalPages, pageNumberMessage, isPrevious, isNext) = await _contactRepository.SearchSortAndPaginationAsync(
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
        [ValidateModel]
        public async Task<IActionResult> CreateContact([FromBody] ContactCreateDto contactDto)
        {
            // Check if a contact with the same email already exists
            var existingContact = await _contactRepository.GetEntityWithSpecAsync(new ContactEmailSpecification(contactDto.Email!));

            if (existingContact != null)
            {
                return Conflict("A contact with this email already exists.");
            }

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
        [ValidateModel]
        public async Task<IActionResult> UpdateContact(Guid id, [FromBody] ContactCreateDto contactDto)
        {
            var existingContact = await _contactRepository.GetByIdAsync(id);

            if (existingContact == null)
            {
                return NotFound();
            }

            // Check if a contact with the same email already exists
            var contactWithSameEmail = await _contactRepository.GetEntityWithSpecAsync(new ContactEmailSpecification(contactDto.Email!));

            if (contactWithSameEmail != null && contactWithSameEmail.Id != id)
            {
                return Conflict("A contact with this email already exists.");
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

            return Ok(new { message = "Contact deleted successfully.", contact });
        }
    }
}
