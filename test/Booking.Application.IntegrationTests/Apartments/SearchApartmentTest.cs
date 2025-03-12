using Booking.Application.Apartments.SearchApartments;
using Booking.Application.IntegrationTests.Infrastructure;
using Booking.Domain.Commons;
using FluentAssertions;

namespace Booking.Application.IntegrationTests.Apartments
{
    public class SearchApartmentTest : ApiCollection
    {
        public SearchApartmentTest(ApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task SearchApartments_ShouldReturnEmptyList_WhenDateRangeInvalid()
        {
            var query = new SearchApartmentsQuery(new DateOnly(2024, 1, 10), new DateOnly(2024, 1, 1));
            Result<IReadOnlyList<ApartmentResponse>> result = await Sender.Send(query);
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchApartments_ShouldReturnApartments_WhenDateRangeIsValid()
        {
            var query = new SearchApartmentsQuery(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
            Result<IReadOnlyList<ApartmentResponse>> result = await Sender.Send(query);
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();
        }
    }
}
