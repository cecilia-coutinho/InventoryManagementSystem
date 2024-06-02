﻿using InventorySystemAPI.DTOs;
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
        public readonly IContactRepository _contactRepository;

        public ContactsController(IContactRepository contactRepository)
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
        [HttpGet("WithFilterSortAndPagination")]
        public async Task<IActionResult> GetContactsWithFilterSortAndPagination(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageIndex = 0,
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            try
            {
                var (result, totalRecordCount, totalPages, isPrevious, isNext) = await _contactRepository.SearchSortAndPaginationAsync(
                    filterOn,
                    filterQuery,
                    sortBy,
                    isDescending,
                    pageIndex,
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
