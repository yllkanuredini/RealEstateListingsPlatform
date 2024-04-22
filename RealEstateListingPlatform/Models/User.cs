﻿using System.ComponentModel.DataAnnotations;

namespace RealEstateListingPlatform.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }
    }
}
