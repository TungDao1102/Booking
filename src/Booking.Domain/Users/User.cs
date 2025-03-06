using Booking.Domain.Abstractions;
using Booking.Domain.Users.Events;

namespace Booking.Domain.Users
{
    public sealed class User : BaseEntity
    {
        private User()
        {

        }
        private User(Guid id, FirstName firstName, LastName lastName, Email email) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
        public FirstName FirstName { get; private set; } = default!;
        public LastName LastName { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public string IdentityId { get; private set; } = string.Empty;

        public static User Create(FirstName firstName, LastName lastName, Email email)
        {
            var user = new User(Guid.NewGuid(), firstName, lastName, email);

            user.AddDomainEvent(new UserCreatedEvent(user.Id));

            return user;
        }

        public void SetIdentityId(string identityId)
        {
            IdentityId = identityId;
        }
    }
}
