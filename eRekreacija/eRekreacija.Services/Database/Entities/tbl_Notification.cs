using eRekreacija.Services.Database.Helper;

namespace eRekreacija.Services.Database.Entities
{
    public class tbl_Notification:IEntity
    {
        public int id {  get; set; }    
        public string name { get; set; }
        public string description { get; set; }
        public DateTimeOffset created_date { get; set; }
        public string user_id { get; set; }
        public tbl_Objects TblObject { get; set; }
        public int object_id { get; set; }
    }
}
