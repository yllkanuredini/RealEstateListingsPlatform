using System.ComponentModel.DataAnnotations;

namespace RealEstateListingPlatform.Models
{
    public class Inquiry
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime InquiryDate { get; set; } = DateTime.Now;

        public int PropertyId { get; set; }
        public Property Property { get; set; }  
        public int UserId { get; set; }
        public User User { get; set; }
        
    }
}
