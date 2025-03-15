namespace eRekreacija.Models.DTOs
{
    public class MyClientPayments
    {
        public string FullName { get; set; }
        public string Email { get; set; } 
        public string Phone { get; set; }
        public string ObjectName { get; set; }
        public float Amount {  get; set; } 
        public DateTime? AppointmentDate { get; set; }
        public string user_id {  get; set; }    
        public int object_id { get; set; }
    }
}