using Booking.Application.Users.LoginUser;
using Booking.Application.Users.RegisterUser;
using Booking.Domain.Commons;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken)
        {
            var query = new GetLoggedInUserQuery();

            Result<UserResponse> result = await _sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }
    }
}
