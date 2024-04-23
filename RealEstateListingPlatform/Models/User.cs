using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RealEstateListingPlatform.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? PasswordHash { get; set; }

        [Compare("PasswordHash", ErrorMessage = "Password and Confirm Password must match")]
        [Required(ErrorMessage = "Confirm Password is required")]
        public string? ConfirmPassword { get; set; }
    }
}