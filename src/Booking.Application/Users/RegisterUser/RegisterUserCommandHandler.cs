using Booking.Application.Abstractions.Authentications;
using Booking.Application.Abstractions.Messaging;
using Booking.Application.Abstractions.Repositories;
using Booking.Domain.Commons;
using Booking.Domain.Users;

namespace Booking.Application.Users.RegisterUser
{
    public class RegisterUserCommandHandler(
        IAuthService authService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<RegisterUserCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = User.Create(new FirstName(request.FirstName),
                                    new LastName(request.LastName),
                                    new Email(request.Email));

            var identityId = await authService.RegisterAsync(user, request.Password, cancellationToken);
            user.SetIdentityId(identityId);
            userRepository.Add(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
