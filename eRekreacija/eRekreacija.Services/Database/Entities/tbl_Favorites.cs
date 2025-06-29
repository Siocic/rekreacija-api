namespace eRekreacija.Services.Database.Entities
{
    public class tbl_Favorites
    {
        public tbl_Objects TblObjects { get; set; }
        public int object_id { get; set; }
        public ApplicationUser User { get; set; }
        public string user_id { get; set; }
    }
}
