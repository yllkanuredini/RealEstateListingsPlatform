using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateListingPlatform.Models
{
    public class PropertyImage
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public Property Property { get; set; }
    }
}
