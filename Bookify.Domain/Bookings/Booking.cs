using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;

namespace Bookify.Domain.Bookings;

public sealed class Booking : Entity
{
    private Booking(
        Guid id,
        Guid apartmentId, 
        Guid userId, 
        DateRange duration, 
        DateTime utcNow)
        : base(id)
    {
        ApartmentId = apartmentId;
        UserId = userId;
        Duration = duration;
        UtcNow = utcNow;
    }

    public Guid AppartmentId { get; private set; }
    public Guid UserId { get; private set; }

    public DateRange Duration { get; private set; }
    public DateTime UtcNow { get; }
    public Money PriceForPeriod { get; private set; }
    public Money CleaningFee { get; private set; }
    public Money AmenitiesUpCharge { get; private set; }
    public Money TotalPrice { get; private set; }
    public BookingStatus Status { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ConfirmedOnUtc { get; private set; }
    public DateTime? RejectedOnUtc { get; private set; }
    public DateTime? CompletedOnUtc { get; private set; }
    public DateTime? CancelledOnUtc { get; private set; }
    public Guid ApartmentId { get; }

    public static Booking Reserve(
        Guid apartmentId,
        Guid userId,
        DateRange duration,
        DateTime utcNow
        )
    {
        /*
         there is an issue that we missed a log of fields(prices) that need to calculated
        calculation of prices is not entity duty so we need to add domain service for calculation
         */
        Booking booking = new(
            Guid.NewGuid(),
            apartmentId,
            userId,
            duration,
            utcNow);


        return booking;
    }
}