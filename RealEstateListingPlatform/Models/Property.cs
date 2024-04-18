using System.ComponentModel.DataAnnotations;

namespace RealEstateListingPlatform.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // e.g., house, apartment, condo
        public string Status { get; set; } // for sale or for rent
        public decimal Price { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int SquareMeters { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<PropertyImage> PropertyImages { get; set; }
        public List<PropertyAmenity> PropertyAmenities { get; set; }
    }
}
