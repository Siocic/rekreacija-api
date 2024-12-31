using Microsoft.AspNetCore.Identity;

namespace eRekreacija.Services.Database
{
    public class ApplicationUser:IdentityUser
    {
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public string? Address {  get; set; }   
        public bool? isApproved { get; set; }=null;
        public byte[]? ProfilePicutre { get; set; }
    }
}
