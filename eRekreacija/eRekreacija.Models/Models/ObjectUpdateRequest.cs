namespace eRekreacija.Models.Models
{
    public  class ObjectUpdateRequest
    {
        public string name { get; set; }
        public DateTime? updated_date { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string? description { get; set; }
        public float price { get; set; }
        public string user_id { get; set; }
        public byte[]? ObjectImage { get; set; } = null;
        public List<int> sportId { get; set; }
    }
}
