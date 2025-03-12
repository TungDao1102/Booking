using Booking.API.Controllers.Users;
using Booking.API.FunctionalTests.Infrastructure;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Booking.API.FunctionalTests.Users
{
    public class RegisterUserTest : ApiCollection
    {
        public RegisterUserTest(ApiFixture factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("", "first", "last", "12345")]
        [InlineData("test.com", "first", "last", "12345")]
        [InlineData("@test.com", "first", "last", "12345")]
        [InlineData("test@", "first", "last", "12345")]
        [InlineData("test@test.com", "", "last", "12345")]
        [InlineData("test@test.com", "first", "", "12345")]
        [InlineData("test@test.com", "first", "last", "")]
        [InlineData("test@test.com", "first", "last", "1")]
        [InlineData("test@test.com", "first", "last", "12")]
        [InlineData("test@test.com", "first", "last", "123")]
        [InlineData("test@test.com", "first", "last", "1234")]
        public async Task Register_ShouldReturnBadRequest_WhenRequestIsInvalid(
        string email,
        string firstName,
        string lastName,
        string password)
        {
            var request = new RegisterUserRequest(email, firstName, lastName, password);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users/register", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenRequestIsValid()
        {
            var request = new RegisterUserRequest("create@test.com", "first", "last", "12345");
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users/register", request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
