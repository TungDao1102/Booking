using Booking.Domain.Apartments;
using Booking.Domain.Bookings;
using Booking.Domain.Commons;
using Booking.Domain.UnitTests.Apartments;
using FluentAssertions;

namespace Booking.Domain.UnitTests.Bookings
{
    public class PricingServiceTest
    {
        [Fact]
        public void CalculatePrice_Should_ReturnCorrectTotalPrice()
        {
            var price = new Money(10.0m, Currency.Usd);
            var period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
            var expectedTotalPrice = new Money(price.Amount * period.LengthInDays, price.Currency);
            Apartment apartment = ApartmentData.Create(price);
            var pricingService = new PricingService();

            PricingDetails pricingDetails = pricingService.CalculatePrice(apartment, period);
            pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
        }

        [Fact]
        public void CalculatePrice_Should_ReturnCorrectTotalPrice_WhenCleaningFeeIsIncluded()
        {
            var price = new Money(10.0m, Currency.Usd);
            var cleaningFee = new Money(99.99m, Currency.Usd);
            var period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
            var expectedTotalPrice = new Money(price.Amount * period.LengthInDays + cleaningFee.Amount, price.Currency);
            Apartment apartment = ApartmentData.Create(price, cleaningFee);
            var pricingService = new PricingService();

            PricingDetails pricingDetails = pricingService.CalculatePrice(apartment, period);
            pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
        }
    }
}
