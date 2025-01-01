namespace eRekreacija.Services.Database.Entities
{
    public class tbl_Appointment
    {
        public int id { get; set; }
        public DateTimeOffset? appointment_date { get; set; }
        public DateTimeOffset? start_time { get; set; }
        public DateTimeOffset? end_time { get;set; }
        public bool? is_approved { get; set; } = null;
        public tbl_Objects TblObjects {  get; set; }    
        public int object_id {  get; set; } 
        public tbl_Payment TblPayment {  get; set; }    
    }
}
