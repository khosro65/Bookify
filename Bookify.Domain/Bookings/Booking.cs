using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
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
        Apartment apartment,
        Guid userId,
        DateRange duration,
        DateTime utcNow,
        PricingService pricingService
        )
    {
        /*
         there is an issue that we missed a log of fields(prices) that need to calculated
        calculation of prices is not entity duty so we need to add domain service for calculation
         */

        PricingDetails pricingDetails = pricingService.CalculatePrice(apartment, duration);

        Booking booking = new(
            Guid.NewGuid(),
            apartment.Id,
            userId,
            duration,
            utcNow,
            pricingDetails.PriceForPeriod,
            pricingDetails.AmenitiesCharge,
            pricingDetails.CleaningFee,
            pricingDetails.TotalPrice,
            BookingStatus.Reserved);

        booking.RaiseDomainEvent(new BookingReserveDomainEvent(booking.Id));

        apartment.LastBookedOnUtc = utcNow;
        return booking;
    }

    public Result Confirm(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
            return Result.Failure(BookingErrors.NotReserved);

        Status = BookingStatus.Confirmed;
        ConfirmedOnUtc = utcNow;

        RaiseDomainEvent(new BookingConfirmedDomainEvent(Id));

        return Result.Success();
    }

    public Result Reject(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
            return Result.Failure(BookingErrors.NotReserved);

        Status = BookingStatus.Rejected;
        RejectedOnUtc = utcNow;

        RaiseDomainEvent(new BookingRejectedDomainEvent(Id));

        return Result.Success();
    }


    public Result Complete(DateTime utcNow)
    {
        if (Status != BookingStatus.Confirmed)
            return Result.Failure(BookingErrors.NotConfirmed);

        Status = BookingStatus.Completed;
        CompletedOnUtc = utcNow;

        RaiseDomainEvent(new BookingCompletedDomainEvent(Id));
        return Result.Success();
    }

    public Result Cancel(DateTime utcNow)
    {
        if (Status != BookingStatus.Confirmed)
            return Result.Failure(BookingErrors.NotConfirmed);

        Status = BookingStatus.Cancelled;
        CancelledOnUtc = utcNow;

        RaiseDomainEvent(new BookingCancelledDomainEvent(Id));

        return Result.Success();
    }
}