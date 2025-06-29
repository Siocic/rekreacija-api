using eRekreacija.Services.Database.Helper;

namespace eRekreacija.Services.Database.Entities
{
    public class tbl_Notification:IEntity
    {
        public int id {  get; set; }    
        public string name { get; set; }
        public string description { get; set; }
        public DateTime created_date { get; set; }
        public ApplicationUser User { get; set; }
        public string user_id { get; set; }    
    }
}
