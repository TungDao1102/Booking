namespace Booking.Application.Abstractions.Clocks
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
