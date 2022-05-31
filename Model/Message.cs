namespace ChatService.Model
{
    public record Message
    {
        public String MessId { get; init; }
        public String User { get; init; }
        public String Text { get; init; }
        public UInt64 Timestamp { get; init; }
    }
}