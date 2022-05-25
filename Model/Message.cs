namespace ChatService.Model
{
    public class Message
    {
        public String MessId { get; set; }
        public String User { get; set; }
        public String Text { get; set; }
        public DateTime Timestamp { get; set; }
    }
}