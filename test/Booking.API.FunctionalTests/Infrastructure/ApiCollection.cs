using Booking.API.Controllers.Users;
using Booking.API.FunctionalTests.Users;
using Booking.Application.Users.LoginUser;
using System.Net.Http.Json;

namespace Booking.API.FunctionalTests.Infrastructure
{
    public abstract class ApiCollection : IClassFixture<ApiFixture>
    {
        protected readonly HttpClient HttpClient;

        protected ApiCollection(ApiFixture factory)
        {
            HttpClient = factory.CreateClient();
        }

        protected async Task<string> GetAccessToken()
        {
            HttpResponseMessage loginResponse = await HttpClient.PostAsJsonAsync(
                "api/v1/users/login",
                new LoginUserRequest(
                    UserData.RegisterTestUserRequest.Email,
                    UserData.RegisterTestUserRequest.Password));

            AccessTokenResponse? accessTokenResponse = await loginResponse.Content.ReadFromJsonAsync<AccessTokenResponse>();

            return accessTokenResponse!.AccessToken;
        }
    }
}
