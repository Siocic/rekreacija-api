namespace eRekreacija.Services.Database.Entities
{
    public class tbl_ChatMessage
    {
        public int Id { get; set; }
        public ApplicationUser Sender { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser Recipient { get; set; }
        public string RecipientId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
