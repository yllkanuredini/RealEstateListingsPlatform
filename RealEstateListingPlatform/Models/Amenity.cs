using System.ComponentModel.DataAnnotations;

namespace RealEstateListingPlatform.Models
{
    public class Amenity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PropertyAmenity> PropertyAmenities { get; set; }
    }
}
