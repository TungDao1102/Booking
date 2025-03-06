using Booking.Application.Abstractions.Authentications;
using Booking.Domain.Users;
using Booking.Infrastructure.Authentications.Models;
using System.Net.Http.Json;

namespace Booking.Infrastructure.Authentications
{
    internal class AuthService(HttpClient httpClient) : IAuthService
    {
        private const string PasswordCredentialType = "password";
        private const string UsersSegmentName = "users/";

        public async Task<string> RegisterAsync(User user, string password, CancellationToken cancellationToken)
        {
            var userRepresentationModel = UserRepresentationModel.FromUser(user);

            userRepresentationModel.Credentials =
            [
                new()
                {
                    Type = PasswordCredentialType,
                    Value = password,
                    Temporary = false
                }
            ];

            var response = await httpClient.PostAsJsonAsync("users", userRepresentationModel, cancellationToken);
            return ExtractIdentityFromLocationHeader(response);
        }

        private static string ExtractIdentityFromLocationHeader(HttpResponseMessage httpResponseMessage)
        {
            string locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery ?? throw new InvalidOperationException("Location header can't be null"); ;

            int userSegmentIndex = locationHeader.IndexOf(UsersSegmentName, StringComparison.InvariantCultureIgnoreCase);

            string userIdentityId = locationHeader[(userSegmentIndex + UsersSegmentName.Length)..];
            return userIdentityId;
        }
    }
}
