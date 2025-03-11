using Asp.Versioning;
using Booking.API.Commons;
using Booking.Application.Reviews;
using Booking.Domain.Commons;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers.Reviews
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiVersion(Versions.V1)]
    public class ReviewsController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddReview(AddReviewRequest request, CancellationToken cancellationToken)
        {
            var command = new AddReviewCommand(request.BookingId, request.Rating, request.Comment);

            Result result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }
    }
}
