using Booking.Domain.Abstractions;
using Booking.Domain.Users.Events;

namespace Booking.Domain.Users
{
    public sealed class User(
        Guid id,
        FirstName firstName,
        LastName lastName,
        Email email) : BaseEntity(id)
    {
        public FirstName FirstName { get; private set; } = firstName;
        public LastName LastName { get; private set; } = lastName;
        public Email Email { get; private set; } = email;

        public static User Create(FirstName firstName, LastName lastName, Email email)
        {
            var user = new User(Guid.NewGuid(), firstName, lastName, email);

            user.AddDomainEvent(new UserCreatedEvent(user.Id));

            return user;
        }
    }
}
