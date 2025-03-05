using Booking.Application.Abstractions.Messaging;
using Booking.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace Booking.Application.Abstractions.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationErrors = _validators
                .Select(v => v.Validate(context))
                .Where(v => v.Errors.Count > 0)
                .SelectMany(result => result.Errors)
                .Select(failure => new ValidationError(failure.PropertyName, failure.ErrorMessage))
                .ToList();

            if (validationErrors.Count > 0)
            {
                throw new Exceptions.ValidationException(validationErrors);
            }
            return await next();
        }
    }
}
