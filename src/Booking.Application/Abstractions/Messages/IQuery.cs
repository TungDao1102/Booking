using Booking.Domain.Commons;
using MediatR;

namespace Booking.Application.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {

    }
}
