using Asp.Versioning;
using Booking.API.Commons;
using Booking.Application.Users.GetLoggedInUser;
using Booking.Application.Users.LoginUser;
using Booking.Application.Users.RegisterUser;
using Booking.Domain.Commons;
using Booking.Infrastructure.Authorizations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers.Users
{
    [ApiController]
    [ApiVersion(Versions.V1, Deprecated = true)]
    [ApiVersion(Versions.V2)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
                request.LastName,
                request.Password);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginUserCommand(request.Email, request.Password);
            Result<AccessTokenResponse> result = await sender.Send(command, cancellationToken);

            return result.IsFailure ? Unauthorized(result.Error) : Ok(result.Value);
        }

        [HttpGet("me")]
        [HasPermission(Permissions.UserRead)]
        [MapToApiVersion(Versions.V1)]
        public async Task<IActionResult> GetLoggedInUserV1(CancellationToken cancellationToken)
        {
            var query = new GetLoggedInUserQuery();

            Result<UserResponse> result = await sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }

        [HttpGet("me")]
        [HasPermission(Permissions.UserRead)]
        [MapToApiVersion(Versions.V2)]
        public async Task<IActionResult> GetLoggedInUserV2(CancellationToken cancellationToken)
        {
            var query = new GetLoggedInUserQuery();

            Result<UserResponse> result = await sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }
    }
}
