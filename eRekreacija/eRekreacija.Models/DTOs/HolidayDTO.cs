using System.Text.Json.Serialization;

namespace eRekreacijaAPI.DTOs
{
    public class HolidayDTO
    {
        public int id { get; set; }
        public string name { get; set; }

        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
    }
}
