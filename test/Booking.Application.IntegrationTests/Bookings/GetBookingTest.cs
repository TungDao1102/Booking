using Booking.Application.Bookings.GetBooking;
using Booking.Application.IntegrationTests.Infrastructure;
using Booking.Domain.Bookings;
using Booking.Domain.Commons;
using FluentAssertions;

namespace Booking.Application.IntegrationTests.Bookings
{
    public class GetBookingTest : ApiCollection
    {
        private static readonly Guid BookingId = Guid.NewGuid();
        public GetBookingTest(ApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetBooking_ShouldReturnFailure_WhenBookingIsNotFound()
        {
            var query = new GetBookingQuery(BookingId);
            Result<BookingResponse> result = await Sender.Send(query);
            result.Error.Should().Be(BookingErrors.NotFound);
        }
    }
}
