using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings;

public sealed class Booking : Entity
{
    private Booking(
        Guid id,
        Guid apartmentId, 
        Guid userId, 
        DateRange duration, 
        DateTime utcNow,
        Money priceForPeriod,
        Money amenitiesCharge,
        Money cleaningFee,
        Money totalPrice,
        BookingStatus bookingStatus)
        : base(id)
    {
        ApartmentId = apartmentId;
        UserId = userId;
        Duration = duration;
        CreatedOnUtc = utcNow;
        PriceForPeriod = priceForPeriod;
        AmenitiesUpCharge = amenitiesCharge;
        CleaningFee = cleaningFee;
        TotalPrice = totalPrice;
        Status = bookingStatus;
    }

    public Guid ApartmentId { get; private set; }
    public Guid UserId { get; private set; }

    public DateRange Duration { get; private set; }
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

    public static Booking Reserve(
        Guid apartmentId,
        Guid userId,
        DateRange duration,
        DateTime utcNow,
        PricingDetails pricingDetails
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
            utcNow,
            pricingDetails.PriceForPeriod,
            pricingDetails.AmenitiesCharge,
            pricingDetails.CleaningFee,
            pricingDetails.TotalPrice,
            BookingStatus.Reserved);

        booking.RaiseDomainEvent(new BookingReserveDomainEvent(booking.Id));
        return booking;
    }
}