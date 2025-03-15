namespace eRekreacija.Models.DTOs
{
    public class MyClientsDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfAppointments { get; set; }
        public DateTime? LastAppointmentDate { get; set; }
    }
}