namespace eRekreacija.Models.DTOs
{
    public class AppointmentDTO
    {
        public int id { get; set; }
        public DateTime? appointment_date { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public int? number_of_players { get; set; }
        public bool? is_approved { get; set; } = false;
        public int object_id { get; set; }
        public string user_id { get; set; }
        public string object_name { get; set; } 
        public string fullname { get; set; }
    }
}