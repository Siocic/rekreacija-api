namespace eRekreacija.Models.DTOs
{
    public class MyReservationDTO
    {
        public string ObjectName { get; set; }
        public string ObjectAdress { get; set; }
        public string ObjectImage { get;set; }
        public DateTime? AppointmentDate { get; set; }
        public DateTime? AppointmentStartDate { get; set; }
        public DateTime? AppointmentEndDate { get; set; }
        public int object_id { get; set; }
        public int? number_of_players { get; set; }
        public double price { get; set; }
        public bool? is_approved { get; set; }
    }
}
