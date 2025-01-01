namespace eRekreacija.Services.Database.Entities
{
    public class tbl_ObjectHoliday
    {
        public tbl_Objects TblObjects { get; set; }
        public int object_id {  get; set; } 

        public tbl_Holiday TblHoliday {  get; set; }    
        public int holiday_id { get; set; }
    }
}
