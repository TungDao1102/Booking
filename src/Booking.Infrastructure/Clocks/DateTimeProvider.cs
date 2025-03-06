using Booking.Application.Abstractions.Clocks;

namespace Booking.Infrastructure.Clocks
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
