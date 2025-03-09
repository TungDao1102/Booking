namespace Booking.Domain.Users.Authorizations
{
    public sealed class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}
