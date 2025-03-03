using Booking.Domain.Abstractions;
using Booking.Domain.Commons;

namespace Booking.Domain.Apartments
{
    public sealed class Apartment(
        Guid id,
        Name name,
        Description description,
        Address address,
        Money price,
        Money cleaningFee,
        ICollection<Amenity> amenities) : BaseEntity(id)
    {
        public Name Name { get; private set; } = name;
        public Description Description { get; private set; } = description;
        public Address Address { get; private set; } = address;
        public Money Price { get; private set; } = price;
        public Money CleaningFee { get; private set; } = cleaningFee;
        public DateTime? LastBooked { get; internal set; }
        public ICollection<Amenity> Amenities { get; set; } = amenities;
    }
}
