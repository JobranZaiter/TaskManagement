using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models.DTOs
{
    public class RegisterReq
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, ErrorMessage = "Name cannot be longer than 30 characters")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long", MinimumLength = 6)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }
}
