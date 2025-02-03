namespace eRekreacija.Models.DTOs
{
    public class ObjectsDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? updated_date { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string? description { get; set; }
        public float price { get; set; }
        public string user_id { get; set; }
        public byte[]? ObjectImage { get; set; }
        public List<int>? sportsId { get; set; }
    }
}
