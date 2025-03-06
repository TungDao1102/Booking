using Booking.Domain.Abstractions;
using Booking.Domain.Commons;

namespace Booking.Domain.Apartments
{
    public sealed class Apartment : BaseEntity
    {
        private Apartment()
        {
        }
        public Apartment(Guid id, Name name, Description description, Address address, Money price, Money cleaningFee, List<Amenity> amenities) : base(id)
        {
            Name = name;
            Description = description;
            Address = address;
            Price = price;
            CleaningFee = cleaningFee;
            Amenities = amenities;
        }

        public Name Name { get; private set; } = default!;
        public Description Description { get; private set; } = default!;
        public Address Address { get; private set; } = default!;
        public Money Price { get; private set; } = default!;
        public Money CleaningFee { get; private set; } = default!;
        public DateTime? LastBooked { get; internal set; }
        public ICollection<Amenity> Amenities { get; set; } = [];
    }
}
