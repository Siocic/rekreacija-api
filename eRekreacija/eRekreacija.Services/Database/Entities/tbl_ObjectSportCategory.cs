namespace eRekreacija.Services.Database.Entities
{
    public class tbl_ObjectSportCategory
    {
        public tbl_Objects TblObject { get; set; }
        public int object_id { get; set; }

        public tbl_SportCategory TblSportcategory { get; set; }
        public int sport_category_id { get; set; }
    }
}
