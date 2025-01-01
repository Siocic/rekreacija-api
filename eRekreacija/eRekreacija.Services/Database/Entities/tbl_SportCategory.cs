using eRekreacija.Services.Database.Helper;

namespace eRekreacija.Services.Database.Entities
{
    public class tbl_SportCategory : IEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public ICollection<tbl_ObjectSportCategory> ObjectSportCategories { get; set; } = new List<tbl_ObjectSportCategory>();
    }
}
