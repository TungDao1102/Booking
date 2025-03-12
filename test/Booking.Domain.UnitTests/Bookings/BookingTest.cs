using Booking.Domain.Apartments;
using Booking.Domain.Bookings.Events;
using Booking.Domain.Bookings;
using Booking.Domain.Commons;
using Booking.Domain.UnitTests.Apartments;
using Booking.Domain.UnitTests.Commons;
using Booking.Domain.UnitTests.Users;
using Booking.Domain.Users;
using FluentAssertions;

namespace Booking.Domain.UnitTests.Bookings
{
    public class BookingTest : BaseTest
    {
        [Fact]
        public void Reserve_Should_RaiseBookingReservedDomainEvent()
        {
            var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
            var price = new Money(10.0m, Currency.Usd);
            var duration = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
            Apartment apartment = ApartmentData.Create(price);
            var pricingService = new PricingService();

            var booking = Domain.Bookings.Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);
            BookingReservedDomainEvent bookingReservedDomainEvent = AssertDomainEventWasPublished<BookingReservedDomainEvent>(booking);
            bookingReservedDomainEvent.BookingId.Should().Be(booking.Id);
        }
    }
}
