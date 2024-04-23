namespace RealEstateListingPlatform.DTOs
{
    public class ViewingDetailDTO
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int UserId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; }
    }
}
