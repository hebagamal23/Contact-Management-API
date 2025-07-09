using System.ComponentModel.DataAnnotations;

namespace Contact_Management_API.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public List<Contact> Contacts { get; set; } = new List<Contact>();

    }
}
