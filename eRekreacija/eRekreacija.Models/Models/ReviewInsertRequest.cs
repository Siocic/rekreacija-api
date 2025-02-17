namespace eRekreacija.Models.Models
{
    public class ReviewInsertRequest
    {
        public string comment { get; set; }
        public float rating { get; set; }
        public DateTime created_date { get; set; }
        public string user_id { get; set; }
        public int object_id { get; set; }
    }
}