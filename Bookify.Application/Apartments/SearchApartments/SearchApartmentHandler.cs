using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Dapper;
using MediatR;

namespace Bookify.Application.Apartments.SearchApartments;

internal sealed class SearchApartmentHandler : IQueryHandler<SearchApartmentQuery, IReadOnlyList<ApartmentResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private static readonly List<int> ActiveBookingStatuses =
        [
            (int)BookingStatus.Reserved,
            (int)BookingStatus.Confirmed,
            (int)BookingStatus.Completed,
        ];
    public SearchApartmentHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(SearchApartmentQuery request, CancellationToken cancellationToken)
    {
        if (request.StartDate > request.EndDate)
            return new List<ApartmentResponse>();


        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql =
            """
            SELECT 
                Id,
                [Name],
                [Description],
                Price_Amount as [Price],
                Price_Currency as [Currency],
                Address_Country as [Country],
                Address_Street as [Street],
                Address_City as [City],
                Address_State as [State],
                Address_ZipCode as [ZipCode]            
            FROM Apartments a 
            WHERE NOT EXISTS
            (
                SELECT 1 FROM Bookings as b
                WHERE
                     b.Id = a.Id AND
                     b.Duration_Start <= @EndDate AND
                     b.Duration_End >= @StartDate AND
                     b.Status in @ActiveBookingStatuses
            )
            """;

        var apartments = await connection.QueryAsync<ApartmentResponse , AddressResponse , ApartmentResponse>(
            sql, 
            (apartment, address) =>
            {
                apartment.Address = address;

                return apartment;
            }
            ,new
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ActiveBookingStatuses = ActiveBookingStatuses
            }
            ,
            splitOn: "Country"
            );

        return apartments.ToList();
    }
}
