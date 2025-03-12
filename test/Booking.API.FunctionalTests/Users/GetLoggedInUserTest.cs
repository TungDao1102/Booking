using Booking.API.FunctionalTests.Infrastructure;
using Booking.Application.Users.GetLoggedInUser;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Booking.API.FunctionalTests.Users
{
    public class GetLoggedInUserTest : ApiCollection
    {
        public GetLoggedInUserTest(ApiFixture factory) : base(factory)
        {
        }

        [Fact]
        public async Task Get_ShouldReturnUnauthorized_WhenAccessTokenIsMissing()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("api/v1/users/me");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_ShouldReturnUser_WhenAccessTokenIsNotMissing()
        {
            string accessToken = await GetAccessToken();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme,
                accessToken);
            UserResponse? user = await HttpClient.GetFromJsonAsync<UserResponse>("api/v1/users/me");
            user.Should().NotBeNull();
        }

    }
}
