using eRekreacija.Services.Database.Helper;

namespace eRekreacija.Services.Database.Entities
{
    public class tbl_Holiday : IEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTimeOffset start_date { get; set; }
        public DateTimeOffset end_date { get; set; }
        public ICollection<tbl_ObjectHoliday> ObjectHolidays { get; set; } = new List<tbl_ObjectHoliday>();
    }
}
