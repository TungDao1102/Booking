namespace Booking.Infrastructure.Outbox
{
    public class OutboxMessage
    {
        public OutboxMessage(Guid id, DateTime occurredOn, string type, string content)
        {
            Id = id;
            OccurredOn = occurredOn;
            Type = type;
            Content = content;
        }
        public Guid Id { get; init; }
        public DateTime OccurredOn { get; init; }
        public string Type { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public DateTime? ProcessedOn { get; init; }
        public string? Error { get; init; }
    }
}
