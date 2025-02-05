namespace eRekreacija.Models.Models
{
    public class NotificationInsertRequest
    {
        public string name { get; set; }
        public string description { get; set; }
        public DateTime created_date { get; set; }
        public string user_id { get; set; }
    }
}
