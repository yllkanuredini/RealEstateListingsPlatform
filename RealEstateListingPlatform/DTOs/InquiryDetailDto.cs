namespace RealEstateListingPlatform.DTOs
{
    public class InquiryDetailDTO
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime InquiryDate { get; set; }
    }
}
