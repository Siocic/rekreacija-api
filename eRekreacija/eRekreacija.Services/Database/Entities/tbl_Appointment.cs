namespace eRekreacija.Services.Database.Entities
{
    public class tbl_Appointment
    {
        public int id { get; set; }
        public DateTime? appointment_date { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get;set; }
        public bool? is_approved { get; set; } = null;
        public tbl_Objects TblObjects {  get; set; }    
        public int object_id {  get; set; } 
        public tbl_Payment TblPayment {  get; set; }    
    }
}
