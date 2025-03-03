namespace eRekreacija.Models.DTOs
{
    public class AppointmentDTO
    {
        public DateTime? appointment_date { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public bool? is_approved { get; set; } = false;
        public int object_id { get; set; }
    }
}