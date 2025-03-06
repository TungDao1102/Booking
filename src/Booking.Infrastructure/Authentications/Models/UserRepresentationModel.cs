using Booking.Domain.Users;

namespace Booking.Infrastructure.Authentications.Models
{
    internal sealed class UserRepresentationModel
    {
        public Dictionary<string, string> Access { get; set; } = default!;

        public Dictionary<string, List<string>> Attributes { get; set; } = default!;

        public Dictionary<string, string> ClientRoles { get; set; } = default!;

        public long? CreatedTimestamp { get; set; }

        public CredentialRepresentationModel[] Credentials { get; set; } = default!;

        public string[] DisableableCredentialTypes { get; set; } = default!;

        public string Email { get; set; } = default!;

        public bool? EmailVerified { get; set; }

        public bool? Enabled { get; set; }

        public string FederationLink { get; set; } = default!;

        public string Id { get; set; } = default!;

        public string[] Groups { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public int? NotBefore { get; set; }

        public string Origin { get; set; } = default!;

        public string[] RealmRoles { get; set; } = default!;

        public string[] RequiredActions { get; set; } = default!;

        public string Self { get; set; } = default!;

        public string ServiceAccountClientId { get; set; } = default!;

        public string Username { get; set; } = default!;

        internal static UserRepresentationModel FromUser(User user) =>
            new()
            {
                FirstName = user.FirstName.Value,
                LastName = user.LastName.Value,
                Email = user.Email.Value,
                Username = user.Email.Value,
                Enabled = true,
                EmailVerified = true,
                CreatedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Attributes = [],
                RequiredActions = []
            };
    }

}
