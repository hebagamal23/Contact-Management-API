using System.ComponentModel.DataAnnotations;

namespace Contact_Management_API.DTOs
{
    public class ContactDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }
    }
}
