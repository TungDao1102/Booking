using Asp.Versioning;
using Asp.Versioning.Builder;
using Booking.API.Controllers.Bookings;
using Booking.Application.Bookings.GetBooking;
using Booking.Application.Bookings.ReserveBooking;
using Booking.Domain.Commons;
using MediatR;

namespace Booking.API.Endpoints
{
    public static class BookingEndpoint
    {
        public static IEndpointRouteBuilder MapBookingEndpoint(this IEndpointRouteBuilder builder)
        {
            ApiVersionSet apiVersionSet = builder.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .HasApiVersion(new ApiVersion(2))
                .ReportApiVersions()
                .Build();

            // using method 1
            builder.MapGet("api/v{version:apiVersion}/bookings/{id}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetBookingQuery(id);
                Result<BookingResponse> result = await sender.Send(query, cancellationToken);
                return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
            }).RequireAuthorization()
            .WithName("GetBooking")
            // .HasApiVersion(1)
            .WithApiVersionSet(apiVersionSet);

            // using method 2
            builder.MapPost("bookings", async (ReserveBookingRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new ReserveBookingCommand(request.ApartmentId, request.UserId, request.StartDate, request.EndDate);

                Result<Guid> result = await sender.Send(command, cancellationToken);

                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.CreatedAtRoute("GetBooking", new { id = result.Value }, result.Value);
            }).RequireAuthorization()
            .WithName(" ReserveBooking");

            return builder;
        }
    }
}
