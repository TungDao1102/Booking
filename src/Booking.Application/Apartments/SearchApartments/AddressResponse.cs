namespace Booking.Application.Apartments.SearchApartments
{
    public sealed class AddressResponse
    {
        public string Country { get; init; } = string.Empty;

        public string State { get; init; } = string.Empty;

        public string ZipCode { get; init; } = string.Empty;

        public string City { get; init; } = string.Empty;

        public string Street { get; init; } = string.Empty;
    }
}
