using System.ComponentModel.DataAnnotations;

namespace RealEstateListingPlatform.Models
{
    public class Viewing
    {
        [Key]
        public int Id { get; set; }
       
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; }

        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
