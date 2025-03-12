using Booking.Application.Bookings.ConfirmBooking;
using Booking.Application.IntegrationTests.Infrastructure;
using Booking.Domain.Bookings;
using Booking.Domain.Commons;
using FluentAssertions;

namespace Booking.Application.IntegrationTests.Bookings
{
    public class ConfirmBookingTest : ApiCollection
    {
        private static readonly Guid BookingId = Guid.NewGuid();
        public ConfirmBookingTest(ApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task ConfirmBooking_ShouldReturnFailure_WhenBookingIsNotFound()
        {
            var command = new ConfirmBookingCommand(BookingId);
            Result result = await Sender.Send(command);
            result.Error.Should().Be(BookingErrors.NotFound);
        }
    }
}
