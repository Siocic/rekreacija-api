namespace eRekreacija.Models.DTOs
{
    public class NotificationDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime created_date { get; set; }
        public string user_id { get; set; }
        public ApplicationUserDTO user { get; set; }
    }
}
