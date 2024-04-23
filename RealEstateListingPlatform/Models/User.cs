using System.ComponentModel.DataAnnotations;

namespace RealEstateListingPlatform.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string? LastName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
        [Required(ErrorMessage = "Confirm Password is required")]
        public string? ConfirmPassword { get; set; }
    }
}
