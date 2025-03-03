namespace eRekreacija.Models.Models
{
    public class AppointmentInsertRequest
    {
        public DateTime? appointment_date { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public bool? is_approved { get; set; } = false;
        public int object_id { get; set; }
        public string user_id { get; set; }
        public float amount { get; set; }
    }
}