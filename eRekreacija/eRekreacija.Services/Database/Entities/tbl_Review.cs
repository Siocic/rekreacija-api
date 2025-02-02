namespace eRekreacija.Services.Database.Entities
{
    public class tbl_Review
    {
        public int id { get; set; }
        public string comment { get; set; }
        public int rating { get; set; }
        public DateTime created_date { get; set; }
        public string user_id { get; set; }
        public tbl_Objects TblObjects {  get; set; }
        public int object_id {  get; set; } 
    }
}
