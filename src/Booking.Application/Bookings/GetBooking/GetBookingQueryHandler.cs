using Booking.Application.Abstractions.Authentications;
using Booking.Application.Abstractions.Data;
using Booking.Application.Abstractions.Messaging;
using Booking.Domain.Bookings;
using Booking.Domain.Commons;
using Dapper;

namespace Booking.Application.Bookings.GetBooking
{
    public class GetBookingQueryHandler(
        ISqlConnectionFactory sqlConnection,
        IUserContext userContext) : IQueryHandler<GetBookingQuery, BookingResponse>
    {
        const string sql = """
            SELECT
                id AS Id,
                apartment_id AS ApartmentId,
                user_id AS UserId,
                status AS Status,
                price_for_period_amount AS PriceAmount,
                price_for_period_currency AS PriceCurrency,
                cleaning_fee_amount AS CleaningFeeAmount,
                cleaning_fee_currency AS CleaningFeeCurrency,
                amenities_up_charge_amount AS AmenitiesUpChargeAmount,
                amenities_up_charge_currency AS AmenitiesUpChargeCurrency,
                total_price_amount AS TotalPriceAmount,
                total_price_currency AS TotalPriceCurrency,
                duration_start AS DurationStart,
                duration_end AS DurationEnd,
                created_on AS CreatedOn
            FROM bookings
            WHERE id = @BookingId
            """;

        public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            using var connection = sqlConnection.CreateConnection();

            var booking = await connection.QuerySingleOrDefaultAsync<BookingResponse>(
               sql, new { request.BookingId });

            if (booking is null || userContext.UserId != booking.UserId)
            {
                return Result.Failure<BookingResponse>(BookingErrors.NotFound);
            }

            return booking;
        }
    }
}
