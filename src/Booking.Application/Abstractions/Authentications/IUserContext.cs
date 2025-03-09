namespace Booking.Application.Abstractions.Authentications
{
    public interface IUserContext
    {
        Guid UserId { get; }

        string IdentityId { get; }
    }
}
