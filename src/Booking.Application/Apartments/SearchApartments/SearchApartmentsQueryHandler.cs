﻿using Booking.Application.Abstractions.Data;
using Booking.Application.Abstractions.Messaging;
using Booking.Domain.Bookings;
using Booking.Domain.Commons;
using Dapper;
using System.Data;

namespace Booking.Application.Apartments.SearchApartments
{
    public class SearchApartmentsQueryHandler(ISqlConnectionFactory sqlConnection) : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<ApartmentResponse>>
    {
        private static readonly int[] ActiveBookingStatuses =
    {
        (int)BookingStatus.Reserved,
        (int)BookingStatus.Confirmed,
        (int)BookingStatus.Completed
    };

        public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(SearchApartmentsQuery request, CancellationToken cancellationToken)
        {
            if (request.StartDate > request.EndDate)
            {
                return new List<ApartmentResponse>();
            }

            using IDbConnection connection = sqlConnection.CreateConnection();

            const string sql = """
            SELECT
                a.id AS Id,
                a.name AS Name,
                a.description AS Description,
                a.price_amount AS Price,
                a.price_currency AS Currency,
                a.address_country AS Country,
                a.address_state AS State,
                a.address_zip_code AS ZipCode,
                a.address_city AS City,
                a.address_street AS Street
            FROM apartments AS a
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM bookings AS b
                WHERE
                    b.apartment_id = a.id AND
                    b.duration_start <= @EndDate AND
                    b.duration_end >= @StartDate AND
                    b.status = ANY(@ActiveBookingStatuses)
            )
            """;

            IEnumerable<ApartmentResponse> apartments = await connection
           .QueryAsync<ApartmentResponse, AddressResponse, ApartmentResponse>(
               sql,
               (apartment, address) =>
               {
                   apartment.Address = address;

                   return apartment;
               },
               new
               {
                   request.StartDate,
                   request.EndDate,
                   ActiveBookingStatuses
               },
               splitOn: "Country");

            return apartments.ToList();
        }
    }
}
