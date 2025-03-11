using Asp.Versioning;
using Booking.API.Commons;
using Booking.Application.Apartments.SearchApartments;
using Booking.Domain.Commons;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers.Apartments
{
    [ApiController]
    [ApiVersion(Versions.V1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ApartmentsController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> SearchApartments(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
        {
            var query = new SearchApartmentsQuery(startDate, endDate);

            Result<IReadOnlyList<ApartmentResponse>> result = await sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }
    }
}
