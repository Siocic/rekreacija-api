using eRekreacija.Services.Database.Helper;
namespace eRekreacija.Services.Database.Entities
{
    public class tbl_Objects : IEntity, IDateEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? updated_date { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string? description { get; set; }
        public float price { get; set; }
        public string user_id { get; set; }
        public byte[]? ObjectImage { get; set; }
        public ICollection<tbl_Appointment> Appointments { get; set; } = new List<tbl_Appointment>();
        public ICollection<tbl_ObjectHoliday> ObjectHoliday { get; set; } = new List<tbl_ObjectHoliday>();
        public ICollection<tbl_Review> Reviews { get; set; } = new List<tbl_Review>();
        public ICollection<tbl_ObjectSportCategory> ObjectSportCategory { get; set; } = new List<tbl_ObjectSportCategory>();
        public ICollection<tbl_Notification> Notifications { get; set; } = new List<tbl_Notification>();
        public ICollection<tbl_Payment> Payment { get; set; } = new List<tbl_Payment>();
    }
}
