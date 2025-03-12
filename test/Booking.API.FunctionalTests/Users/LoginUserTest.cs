using Booking.API.Controllers.Users;
using Booking.API.FunctionalTests.Infrastructure;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Booking.API.FunctionalTests.Users
{
    public class LoginUserTest : ApiCollection
    {
        private const string Email = "login@test.com";
        private const string Password = "12345";

        public LoginUserTest(ApiFixture factory) : base(factory)
        {
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenUserDoesNotExist()
        {
            var request = new LoginUserRequest(Email, Password);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users/login", request);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenUserExists()
        {
            var registerRequest = new RegisterUserRequest(Email, "first", "last", Password);
            await HttpClient.PostAsJsonAsync("api/v1/users/register", registerRequest);

            var request = new LoginUserRequest(Email, Password);
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users/login", request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
