using Contact_Management_API.Data;
using Contact_Management_API.DTOs;
using Contact_Management_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using PhoneNumbers;

namespace Contact_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactsController(AppDbContext context)
        {
            _context = context;
        }

        // Helper: get logged in user id from token

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // POST: api/contacts
        [HttpPost]
        public async Task<IActionResult> AddContact(ContactDto dto)
        {
            // ✅ VALIDATION
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                return BadRequest(new { status = 400, message = "First name is required." });

            if (string.IsNullOrWhiteSpace(dto.LastName))
                return BadRequest(new { status = 400, message = "Last name is required." });

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { status = 400, message = "Email is required." });

            if (!dto.Email.Contains("@") || !dto.Email.Contains("."))
                return BadRequest(new { status = 400, message = "Invalid email format." });

            if (dto.BirthDate > DateTime.Today)
                return BadRequest(new { status = 400, message = "Birth date cannot be in the future." });

            // ✅ Phone number format check
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                return BadRequest(new { status = 400, message = "Phone number is required." });

            if (!dto.PhoneNumber.StartsWith("+"))
                return BadRequest(new { status = 400, message = "Phone number must start with '+' and include country code." });

            var phoneUtil = PhoneNumberUtil.GetInstance();
            try
            {
                var parsedNumber = phoneUtil.Parse(dto.PhoneNumber, null);
                if (!phoneUtil.IsValidNumber(parsedNumber))
                {
                    return BadRequest(new { status = 400, message = "Invalid phone number." });
                }

                // Format the number in international format (E.164)
                dto.PhoneNumber = phoneUtil.Format(parsedNumber, PhoneNumberFormat.E164);
            }
            catch (NumberParseException)
            {
                return BadRequest(new { status = 400, message = "Invalid phone number format." });
            }

            var userId = GetUserId();

            // ✅ Prevent duplicate email for same user
            bool emailExists = await _context.Contacts
                .AnyAsync(c => c.Email.ToLower() == dto.Email.ToLower().Trim() && c.UserId == userId);

            if (emailExists)
                return Conflict(new { status = 409, message = "You already added this email." });

            var contact = new Contact
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName?.Trim(),
                PhoneNumber = dto.PhoneNumber, // already formatted
                Email = dto.Email.Trim().ToLower(),
                BirthDate = dto.BirthDate,
                UserId = userId
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return Ok(new { status = 200, message = "Contact added successfully." });
        }

        // GET: api/contacts
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            try
            {
                var userId = GetUserId();

                var contacts = await _context.Contacts
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                if (contacts == null || contacts.Count == 0)
                {
                    return NotFound(new
                    {
                        status = 404,
                        message = "No contacts found for this user."
                    });
                }

                return Ok(new
                {
                    status = 200,
                    message = "Contacts retrieved.",
                    data = contacts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    message = "An error occurred while retrieving contacts.",
                    error = ex.Message
                });
            }
        }

        // GET: api/contacts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { status = 400, message = "Invalid contact ID." });

                var userId = GetUserId();

                var contact = await _context.Contacts
                    .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

                if (contact == null)
                    return NotFound(new { status = 404, message = "Contact not found." });

                return Ok(new
                {
                    status = 200,
                    message = "Contact found.",
                    data = contact
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    message = "An error occurred while retrieving the contact.",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/contacts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { status = 400, message = "Invalid contact ID." });

                var contact = await _context.Contacts
                    .FirstOrDefaultAsync(c => c.Id == id && c.UserId == GetUserId());

                if (contact == null)
                    return NotFound(new { status = 404, message = "Contact not found." });

                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();

                return Ok(new { status = 200, message = "Contact deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    message = "An error occurred while deleting the contact.",
                    error = ex.Message
                });
            }
        }
    }
}
