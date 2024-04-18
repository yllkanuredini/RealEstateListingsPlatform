using System.ComponentModel.DataAnnotations;

namespace RealEstateListingPlatform.Models
{
    public class PropertyImage
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
