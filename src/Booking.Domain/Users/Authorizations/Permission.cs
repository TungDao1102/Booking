namespace Booking.Domain.Users.Authorizations
{
    public sealed class Permission
    {
        public static readonly Permission UserRead = new(1, "user:read");
        public Permission(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
