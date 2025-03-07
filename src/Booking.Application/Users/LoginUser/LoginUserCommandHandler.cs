using Booking.Application.Abstractions.Authentications;
using Booking.Application.Abstractions.Messaging;
using Booking.Domain.Commons;
using Booking.Domain.Users;

namespace Booking.Application.Users.LoginUser
{
    internal sealed class LoginUserCommandHandler(IJwtService jwtService) : ICommandHandler<LoginUserCommand, AccessTokenResponse>
    {
        public async Task<Result<AccessTokenResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await jwtService.GetAccessTokenAsync(request.Email, request.Password, cancellationToken);
            if (result.IsFailure)
            {
                return Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials);
            }
            return new AccessTokenResponse(result.Value);
        }
    }
}
