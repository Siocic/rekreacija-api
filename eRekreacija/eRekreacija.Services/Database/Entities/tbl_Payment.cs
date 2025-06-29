namespace eRekreacija.Services.Database.Entities
{
    public class tbl_Payment
    {
        public int id { get; set; }
        public float amount { get; set; }
        public DateTime? paid_date { get; set; }
        public ApplicationUser User { get; set; }
        public string user_id { get; set; }
        public tbl_Appointment TblAppointment {  get; set; }
        public int appointment_id {  get; set; }    
        public tbl_Objects TblObjects { get; set; }
        public int object_id { get; set; }
    }
}
