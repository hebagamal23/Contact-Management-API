using Contact_Management_API.Data;
using Contact_Management_API.DTOs;
using Contact_Management_API.Models;
using Contact_Management_API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Contact_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _hasher;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, IPasswordHasher<User> hasher, TokenService tokenService)
        {
            _context = context;
            _hasher = hasher;
            _tokenService = tokenService;
        }

        private bool IsStrongPassword(string password)
        {
            if (password.Length < 8) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (!password.Any(char.IsDigit)) return false;
            return true;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            dto.FullName = dto.FullName?.Trim();
            dto.Email = dto.Email?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(dto.FullName) || dto.FullName.Length < 3)
                return BadRequest(new { status = 400, message = "Full name must be at least 3 characters." });

            if (!new EmailAddressAttribute().IsValid(dto.Email))
                return BadRequest(new { status = 400, message = "Invalid email format." });

            if (!IsStrongPassword(dto.Password))
                return BadRequest(new { status = 400, message = "Password must be at least 8 characters, with at least one number and one uppercase letter." });

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { status = 400, message = "Email is already in use." });

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { status = 200, message = "Registration successful." });
        }



        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            dto.Email = dto.Email?.Trim().ToLower();

            if (!new EmailAddressAttribute().IsValid(dto.Email))
                return BadRequest(new { status = 400, message = "Invalid email format." });

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { status = 400, message = "Password is required." });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return Unauthorized(new { status = 401, message = "Invalid email or password." });

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { status = 401, message = "Invalid email or password." });

            var token = _tokenService.CreateToken(user);

            return Ok(new
            {
                status = 200,
                message = "Login successful.",
                token
            });
        }


    }
}
