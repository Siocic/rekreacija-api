using eRekreacija.Services.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace eRekreacija.Services.Database
{
    public class ApplicationUser:IdentityUser
    {
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public string? Address {  get; set; }
        public string? City { get; set; }
        public bool? isApproved { get; set; }=null;
        public byte[]? ProfilePicutre { get; set; }
        public ICollection<tbl_Review> Reviews { get; set; } = new List<tbl_Review>();
        public ICollection<tbl_Objects> Objects { get; set; } = new List<tbl_Objects>();
        public ICollection<tbl_Appointment> Appointments { get; set; } = new List<tbl_Appointment>();
        public ICollection<tbl_Notification> Notifications { get; set; } = new List<tbl_Notification>();
        public ICollection<tbl_Payment> Payment { get; set; } = new List<tbl_Payment>();
        public ICollection<tbl_Favorites> Favorites { get; set; } = new List<tbl_Favorites>();
        public ICollection<tbl_ChatMessage> SentMessges { get; set; } = new List<tbl_ChatMessage>();
        public ICollection<tbl_ChatMessage> RecievedMessage { get; set; } = new List<tbl_ChatMessage>();
    }
}
