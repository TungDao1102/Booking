using Booking.Domain.Commons;

namespace Booking.Domain.Bookings
{
    public sealed record PricingDetails(Money PriceForPeriod, Money CleaningFee, Money AmenitiesUpCharge, Money TotalPrice);
}
