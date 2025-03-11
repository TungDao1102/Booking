using Asp.Versioning;
using Booking.API.Commons;
using Booking.Application.Bookings.GetBooking;
using Booking.Application.Bookings.ReserveBooking;
using Booking.Domain.Commons;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers.Bookings
{
    [ApiController]
    [ApiVersion(Versions.V1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BookingsController(ISender sender) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetBookingQuery(id);

            Result<BookingResponse> result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ReserveBooking(ReserveBookingRequest request,
        CancellationToken cancellationToken)
        {
            var command = new ReserveBookingCommand(request.ApartmentId, request.UserId, request.StartDate, request.EndDate);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetBooking), new { id = result.Value }, result.Value);
        }
    }
}
