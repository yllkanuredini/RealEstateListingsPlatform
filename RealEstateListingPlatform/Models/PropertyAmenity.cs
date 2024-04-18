namespace RealEstateListingPlatform.Models
{
    public class PropertyAmenity
    {
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public int AmenityId { get; set; }
        public Amenity Amenity { get; set; }
    }
}
