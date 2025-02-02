namespace eRekreacija.Models.Models
{
    public  class ObjectUpdateRequest
    {
        public int id { get; set; }
        public string name { get; set; }
        //public DateTimeOffset? created_date { get; set; }
        public DateTimeOffset? updated_date { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string? description { get; set; }
        public float price { get; set; }
        public string user_id { get; set; }
    }
}
