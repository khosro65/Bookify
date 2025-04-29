using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Dapper;

namespace Bookify.Application.Bookings.GetBooking;

internal sealed class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetBookingQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {

        using var conneciton = _sqlConnectionFactory.CreateConnection();

        const string sql =
            """
            SELECT * FROM Bookings WHERE Id = @Id
            """;

        var booking = await conneciton.QueryFirstOrDefaultAsync<BookingResponse>(
            sql,
            new { Id = request.BookingId });

        return booking;
    }
}
