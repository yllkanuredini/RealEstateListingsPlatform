﻿namespace RealEstateListingPlatform.DTOs
{
    public class PropertyDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int SquareMeters { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<PropertyImageDto> PropertyImages { get; set; }
        public List<PropertyAmenityDto> PropertyAmenities { get; set; }
    }
}
