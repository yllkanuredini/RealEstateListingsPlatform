using System.ComponentModel.DataAnnotations;

namespace RealEstateListingPlatform.Models
{
    public class User
    {
<<<<<<< HEAD
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string? LastName { get; set; }
=======
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }
>>>>>>> c695abaf09da1f3694ce8ba9d32d85a09cd12367

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
        [Required(ErrorMessage = "Confirm Password is required")]
        public string? ConfirmPassword { get; set; }
    }
}
