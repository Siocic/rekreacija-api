namespace eRekreacija.Models.DTOs
{
    public class ApplicationUserDTO
    {
        public string? Id {  get; set; }    
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? ProfilePicture { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
